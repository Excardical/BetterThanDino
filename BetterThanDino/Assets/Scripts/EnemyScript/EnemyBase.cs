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
    [SerializeField] private Vector2 detectionOffset; // New offset variable

    // Combat variables
    [Header("Combat Settings")]
    public int damage = 1;
    public Transform attackPoint;
    public float weaponRange = 1f;
    public float knockbackForce = 5f;
    public float knockbackTime = 0.5f;
    public float stunTime = 0.5f;

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
        rb.velocity = Vector2.left * speed;
        ChangeState(EnemyState.MoveLeft);
    }

    private void Chase()
    {
        if (target == null) return;

        // Get direction to target
        Vector2 direction = (target.position - transform.position).normalized;

        // Only use the x component for movement
        Vector2 horizontalMovement = new Vector2(direction.x, 0) * speed;
        rb.velocity = horizontalMovement;
    }

    // Combat
    public void Attack()
    {
        if (attackPoint == null)
        {
            Debug.LogError("AttackPoint is not assigned. Cannot perform attack!");
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);

        foreach (var hit in hits)
        {
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            playerHealth?.ChangeHealth(-damage);

            PlayerMovement playerMovement = hit.GetComponent<PlayerMovement>();
            playerMovement?.Knockback(transform, knockbackForce, knockbackTime);
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
        rb.velocity = direction * knockbackForce;

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

        Vector2 detectionPosition = (Vector2)transform.position + detectionOffset; // Calculate detection position
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPosition, playerDetectRange, playerLayer);

        if (hits.Length > 0)
        {
            target = hits[0].transform;

            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            if (distanceToTarget <= attackRange && attackCooldownTimer <= 0)
            {
                ChangeState(EnemyState.Attacking);
                attackCooldownTimer = attackCooldown;
                StartCoroutine(PostAttackPause());
            }
            else if (distanceToTarget > attackRange)
            {
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

    private void Die()
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
        }
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
    MoveLeft
}