using UnityEngine;
using System.Collections;

public class Nightborne : EnemyBase
{
    [Header("Explosion Settings")]
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private int explosionDamage = 3;
    [SerializeField] private LayerMask explosionTargets;
    [SerializeField] private float deathDelay = 1f; // Time before explosion
    [SerializeField] private float destroyDelay = 2f; // Time after explosion before destroying
    private bool hasDied = false;
    private Animator animator;
    private Rigidbody2D rb;
    
    void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (animator == null)
        {
            Debug.LogError("Animator component missing on Nightborne!");
        }
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing on Nightborne!");
        }
    }

    void Update()
    {
        base.Update();
    }

    protected override void Die()
    {
        if (!hasDied)
        {
            hasDied = true;
            ChangeState(EnemyState.Death);
            StartCoroutine(DeathSequence());
        }
    }
    
    private IEnumerator DeathSequence()
    {
        // Disable collider but keep trigger active for explosion
        var collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
        
        // Freeze the rigidbody
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic; // Switch to Kinematic to prevent falling
            rb.simulated = false; // Disable physics simulation
        }
        
        // Trigger death animation
        animator.SetTrigger("Death");
        
        // Wait for death animation
        yield return new WaitForSeconds(deathDelay);
        
        // Trigger explosion
        Explode();
        
        // Wait before destroying the object
        yield return new WaitForSeconds(destroyDelay);
        
        // Finally destroy the object
        Destroy(gameObject);
    }
    
    public override void TriggerExplosion()
    {
        Explode();
    }

    private void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionTargets);

        foreach (var hit in hits)
        {
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(-explosionDamage);
            }

            EnemyBase enemy = hit.GetComponent<EnemyBase>();
            if (enemy != null && enemy != this)
            {
                enemy.TakeDamage(explosionDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}