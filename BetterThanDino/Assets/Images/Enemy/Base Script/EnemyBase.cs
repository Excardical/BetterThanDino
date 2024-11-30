using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float walkSpeed = 1f; // Walking speed
    public float health = 100f; // Shared health
    public float attackPower = 20f; // Shared attack power
    public LayerMask playerLayer; // Layer to detect the player
    public float detectionRadius = 3f; // Radius to detect the player

    protected Animator animator;
    protected Rigidbody2D rb;
    protected bool isAttacking = false; // Prevent redundant attacks
    private float spawnTime;

    // Pre-allocate an array for OverlapCircleNonAlloc
    private Collider2D[] detectedColliders = new Collider2D[1];

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spawnTime = Time.time;

        if (animator == null)
        {
            Debug.LogError($"Animator is missing on {gameObject.name}!");
        }
        else
        {
            animator.SetBool("isWalking", true);
        }
    }

    protected virtual void Update()
    {
        // Handle death if lifetime expires
        if (health <= 0f)
        {
            Die();
            return;
        }

        // Walk if not attacking
        if (!isAttacking)
        {
            WalkLeft();
        }

        // Detect player and attack if detected
        int detectedCount = Physics2D.OverlapCircleNonAlloc(transform.position, detectionRadius, detectedColliders, playerLayer);
        if (detectedCount > 0 && !isAttacking)
        {
            Attack();
        }
    }

    protected virtual void WalkLeft()
    {
        rb.velocity = new Vector2(-walkSpeed, rb.velocity.y);
        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }
    }

    protected virtual void Attack()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero; // Stop movement
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetTrigger("Attack");
        }
    }

    protected virtual void Die()
    {
        rb.velocity = Vector2.zero; // Stop movement
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        Destroy(gameObject, 1.5f); // Destroy after death animation
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize detection radius in Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
