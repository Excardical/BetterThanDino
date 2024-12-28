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
    
    void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component missing on Nightborne!");
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
        
        // Get the base class's rb reference
        var baseRb = GetComponent<Rigidbody2D>();
        if (baseRb != null)
        {
            baseRb.velocity = Vector2.zero;
            baseRb.bodyType = RigidbodyType2D.Kinematic; // Switch to Kinematic to prevent falling
            baseRb.simulated = false; // Disable physics simulation
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

            // Add check for ally
            AllyBase ally = hit.GetComponent<AllyBase>();  // Use your actual ally script name here
            if (ally != null)
            {
                ally.TakeDamage(explosionDamage, transform);  // Passing the Nightborne's transform for knockback direction
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}