using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyBase : MonoBehaviour
{
    // Health variables
    [Header("Health Settings")]
    public int maxHealth = 10;
    private int currentHealth;

    // Movement variables
    [Header("Movement Settings")]
    public float speed = 2f;
    public float attackRange = 2f;
    public float enemyDetectRange = 5f;
    public float attackCooldown = 2f;
    public LayerMask enemyLayer;
    [SerializeField] private Vector2 detectionOffset;
    public LayerMask playerLayer;

    // Combat variables
    [Header("Combat Settings")]
    public int damage = 1;
    public Transform attackPoint;
    public float weaponRange = 1f;
    public float knockbackTime = 0.5f;
    public float stunTime = 0.5f;

    // State management
    private AllyState allyState;
    private float attackCooldownTimer;

    // Components
    private Rigidbody2D rb;
    private Transform target;
    private Animator anim;

    private void Start()
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
        
        // Ignore collision with Player
        Collider2D[] playerColliders = FindObjectsOfType<Collider2D>();
        foreach (var collider in playerColliders)
        {
            if (((1 << collider.gameObject.layer) & playerLayer) != 0)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider);
            }
        }

        ChangeState(AllyState.MoveRight);
    }

    private void Update()
    {
        if (allyState == AllyState.Knockback) return;

        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        switch (allyState)
        {
            case AllyState.Idle:
                rb.velocity = Vector2.zero;
                break;
            case AllyState.Chasing:
                Chase();
                break;
            case AllyState.Attacking:
                rb.velocity = Vector2.zero;
                break;
            case AllyState.MoveRight:
                MoveRight();
                break;
        }

        CheckForEnemy();
    }

    // Movement
    private void MoveRight()
    {
        Vector2 currentVelocity = rb.velocity;
        rb.velocity = new Vector2(speed, currentVelocity.y); // Move right
        ChangeState(AllyState.MoveRight);
    }

    private void Chase()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y); // Horizontal chase
    }

    // Combat
    public void Attack()
    {
        if (attackPoint == null)
        {
            Debug.LogError("AttackPoint is not assigned. Cannot perform attack!");
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemyLayer);

        foreach (var hit in hits)
        {
            Debug.Log("Attacking enemy: " + hit.gameObject.name); // Debug log to show which object is being attacked

            EnemyBase enemy = hit.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    // Enemy detection
    private void CheckForEnemy()
    {
        Vector2 detectionPosition = (Vector2)transform.position + detectionOffset;
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPosition, enemyDetectRange, enemyLayer);

        if (hits.Length > 0)
        {
            Debug.Log("Detected enemy: " + hits[0].gameObject.name);
            target = hits[0].transform;

            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            if (distanceToTarget <= attackRange && attackCooldownTimer <= 0)
            {
                ChangeState(AllyState.Attacking);
                attackCooldownTimer = attackCooldown;
                Attack();
            }
            else if (distanceToTarget > attackRange)
            {
                ChangeState(AllyState.Chasing);
            }
        }
        else
        {
            ChangeState(AllyState.MoveRight);
        }
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

    public void TakeDamage(int damage, Transform attackerTransform, float knockbackForce = 7f, float stunDuration = 2f)    
    {
        Debug.Log($"TakeDamage called. Damage: {damage}, KnockbackForce: {knockbackForce}");
        ChangeHealth(-damage);
        if (currentHealth > 0)
        {
            ApplyKnockback(attackerTransform, knockbackForce, stunDuration);
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
    
    // Knockback Function
    public void ApplyKnockback(Transform attackerTransform, float knockbackForce, float stunDuration)
    {
        // Prevent knockback if health is zero or below
        if (currentHealth <= 0) return;

        ChangeState(AllyState.Knockback); // Ensure the ally enters the Knockback state
        StartCoroutine(KnockbackRoutine(attackerTransform, knockbackForce, stunDuration));
    }

    private IEnumerator KnockbackRoutine(Transform attackerTransform, float knockbackForce, float stunDuration)
    {
        if (attackerTransform == null) yield break;

        // Calculate direction away from the attacker
        Vector2 direction = (transform.position - attackerTransform.position).normalized;

        // Apply knockback velocity
        rb.velocity = direction * knockbackForce;

        // Wait for knockback duration
        yield return new WaitForSeconds(knockbackTime);

        // Stop movement
        rb.velocity = Vector2.zero;

        // Wait for stun duration before regaining control
        yield return new WaitForSeconds(stunDuration);

        // Return to idle if health is above zero
        if (currentHealth > 0)
        {
            ChangeState(AllyState.Idle);
        }
    }
    
    // State management
    public void ChangeState(AllyState newState)
    {
        if (anim != null)
        {
            anim.SetBool("isIdle", false);
            anim.SetBool("isChasing", false);
            anim.SetBool("isAttacking", false);
        }

        allyState = newState;

        switch (newState)
        {
            case AllyState.Idle:
                anim?.SetBool("isIdle", true);
                break;
            case AllyState.Chasing:
                anim?.SetBool("isChasing", true);
                break;
            case AllyState.Attacking:
                anim?.SetBool("isAttacking", true);
                break;
            case AllyState.MoveRight:
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

        Gizmos.color = Color.green;
        Vector2 detectionPosition = (Vector2)transform.position + detectionOffset;
        Gizmos.DrawWireSphere(detectionPosition, enemyDetectRange);
    }
}

// Ally states
public enum AllyState
{
    Idle,
    Chasing,
    Attacking,
    Knockback,
    MoveRight, 
    Death 
}
