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
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.MoveLeft);
    }

    public void Update()
    {
        if (enemyState != EnemyState.Knockback)
        {
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
    }

    // Movement
    private void MoveLeft()
    {
        rb.velocity = Vector2.left * speed;
        ChangeState(EnemyState.MoveLeft);
    }

    private void Chase()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    // Combat
    public void Attack()
    {
        Debug.Log("Enemy Attack method called");
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);
        Debug.Log($"Found {hits.Length} player hits");

        if (hits.Length > 0)
        {
            var playerHealth = hits[0].GetComponent<PlayerHealth>();
            Debug.Log($"PlayerHealth component found: {playerHealth != null}");
        
            playerHealth?.ChangeHealth(-damage);
            hits[0].GetComponent<PlayerMovement>()?.Knockback(transform, knockbackForce, knockbackTime);
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
        if (currentHealth <= 0) yield break; // Exit if the enemy is already dead

        Vector2 direction = (transform.position - playerTransform.position).normalized;
        rb.velocity = direction * knockbackForce;

        yield return new WaitForSeconds(knockbackTime);

        if (currentHealth <= 0) yield break; // Exit if the enemy dies during knockback

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

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, playerDetectRange, playerLayer);

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
            else if (distanceToTarget > attackRange && enemyState != EnemyState.Attacking)
            {
                ChangeState(EnemyState.Chasing);
            }
        }
        else if (enemyState != EnemyState.Attacking)
        {
            ChangeState(EnemyState.MoveLeft);
        }
    }

    private IEnumerator PostAttackPause()
    {
        isPostAttackPausing = true;

        // Wait for attack animation to complete
        float attackAnimationLength = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(attackAnimationLength);

        ChangeState(EnemyState.Idle);
        yield return new WaitForSeconds(0.5f); // Slight delay for recovery
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
        if (currentHealth <= 0) return;
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    // State management
    public void ChangeState(EnemyState newState)
    {
        anim?.SetBool("isIdle", false);
        anim?.SetBool("isChasing", false);
        anim?.SetBool("isAttacking", false);

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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectRange);
        Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
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
