using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // Health variables
    [Header("Health Settings")]
    public int maxHealth = 10;
    private int currentHealth;

    // Movement variables
    [Header("Movement Settings")]
    public float speed = 2f;
    public float attackRange = 2f;
    public float playerDetectRange = 5f;
    public float attackCooldown = 2f;
    public LayerMask playerLayer;
    public LayerMask baseLayer;
    public LayerMask allyLayer;
    [SerializeField] private Vector2 detectionOffset; // New offset variable

    // Combat variables
    [Header("Combat Settings")]
    public int damage = 1;
    public Transform attackPoint;
    public float weaponRange = 1f;
    public float knockbackTime = 0.5f;
    public float stunTime = 0.5f;
    public float attackKnockbackForce = 5f; // Force applied TO player
    public float receivedKnockbackForce = 5f; // Force applied TO enemy

    // State management
    private EnemyState enemyState;
    private float attackCooldownTimer;
    private bool isPostAttackPausing = false;

    // Components
    private Rigidbody2D rb;
    private Transform target;
    private Animator anim;

    public void Start()
    {
        // Initialization
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing!");
        }

        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator component is missing!");
        }

        if (attackPoint == null)
        {
            Debug.LogError("AttackPoint Transform is not assigned in the Inspector!");
        }

        ChangeState(EnemyState.MoveLeft);
    }

    public void Update()
    {
        if (enemyState == EnemyState.Knockback) return;

        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        switch (enemyState)
        {
            case EnemyState.Idle:
                rb.velocity = Vector2.zero;
                break;
            case EnemyState.Chasing:
                Chase();
                break;
            case EnemyState.Attacking:
                rb.velocity = Vector2.zero;
                break;
            case EnemyState.MoveLeft:
                MoveLeft();
                break;
        }

        CheckForPlayer();
    }

    // Movement
    private void MoveLeft()
    {
        Vector2 currentVelocity = rb.velocity;
        rb.velocity = new Vector2(-speed, currentVelocity.y); // Keep existing y velocity
        ChangeState(EnemyState.MoveLeft);
    }

    private void Chase()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;

        // Preserve the current y velocity while chasing horizontally
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
    }


    // Combat
    public void Attack()
    {
        if (attackPoint == null)
        {
            Debug.LogError("AttackPoint is not assigned. Cannot perform attack!");
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer | baseLayer);

        foreach (var hit in hits)
        {
            Debug.Log("Attacking: " + hit.gameObject.name); // Debug log to show which object is being attacked

            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(-damage);
                PlayerMovement playerMovement = hit.GetComponent<PlayerMovement>();
                playerMovement?.Knockback(transform, attackKnockbackForce, knockbackTime);
            }

            BaseHealth baseHealth = hit.GetComponent<BaseHealth>();
            if (baseHealth != null)
            {
                baseHealth.ChangeHealth(-damage);
            }
            
            // Add check for AllyBase
            AllyBase ally = hit.GetComponent<AllyBase>();
            if (ally != null)
            {
                ally.TakeDamage(damage, transform, attackKnockbackForce, stunTime);
            }
        }
    }

    // Knockback
    public void ApplyKnockback(Transform playerTransform)
    {
        if (currentHealth <= 0) return;

        ChangeState(EnemyState.Knockback);
        StartCoroutine(KnockbackRoutine(playerTransform));
    }

    private IEnumerator KnockbackRoutine(Transform playerTransform)
    {
        if (playerTransform == null) yield break;

        Vector2 direction = (transform.position - playerTransform.position).normalized;
        rb.velocity = direction * receivedKnockbackForce;

        yield return new WaitForSeconds(knockbackTime);

        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(stunTime);

        if (currentHealth > 0)
        {
            ChangeState(EnemyState.Idle);
        }
    }

    // Player detection
    private void CheckForPlayer()
    {
        if (isPostAttackPausing) return;

        Vector2 detectionPosition = (Vector2)transform.position + detectionOffset;
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPosition, playerDetectRange, playerLayer | baseLayer);

        if (hits.Length > 0)
        {
            Debug.Log("Detected target: " + hits[0].gameObject.name); // Debug log to show detected target
            target = hits[0].transform;

            float distanceToTarget = Vector2.Distance(transform.position, target.position);
            Debug.Log($"Distance to target ({target.name}): {distanceToTarget}, Attack Range: {attackRange}");

            if (distanceToTarget <= attackRange && attackCooldownTimer <= 0)
            {
                Debug.Log($"Enemy is attacking target: {target.name}");
                ChangeState(EnemyState.Attacking);
                attackCooldownTimer = attackCooldown;
                StartCoroutine(PostAttackPause());
            }
            else if (distanceToTarget > attackRange)
            {
                Debug.Log($"Target ({target.name}) is out of attack range. Chasing.");
                ChangeState(EnemyState.Chasing);
            }
        }
        else
        {
            ChangeState(EnemyState.MoveLeft);
        }
    }

    private IEnumerator PostAttackPause()
    {
        isPostAttackPausing = true;

        float attackAnimationLength = anim != null ? anim.GetCurrentAnimatorStateInfo(0).length : 0;
        yield return new WaitForSeconds(attackAnimationLength);

        ChangeState(EnemyState.Idle);
        yield return new WaitForSeconds(0.5f);

        isPostAttackPausing = false;
    }

    // Health management
    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        ChangeHealth(-damage);
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    // State management
    public void ChangeState(EnemyState newState)
    {
        if (anim != null)
        {
            anim.SetBool("isIdle", false);
            anim.SetBool("isChasing", false);
            anim.SetBool("isAttacking", false);
        }

        enemyState = newState;

        switch (newState)
        {
            case EnemyState.Idle:
                anim?.SetBool("isIdle", true);
                OnIdleStateEnter();
                break;
            case EnemyState.Chasing:
                anim?.SetBool("isChasing", true);
                break;
            case EnemyState.Attacking:
                anim?.SetBool("isAttacking", true);
                break;
            case EnemyState.MoveLeft:
                anim?.SetBool("isChasing", true);
                break;
            case EnemyState.Death:
                // Death is handled via trigger 
                break;
        }
    }

    // Addition for Child Script Functions
    protected virtual void OnIdleStateEnter()
    {
        // Empty base implementation
    }

    public virtual void TriggerExplosion()
    {
        // Empty base implementation
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
        }

        Gizmos.color = Color.blue;
        Vector2 detectionPosition = (Vector2)transform.position + detectionOffset;
        Gizmos.DrawWireSphere(detectionPosition, playerDetectRange);
    }
}

// Enemy states
public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    Knockback,
    MoveLeft,
    Death
}