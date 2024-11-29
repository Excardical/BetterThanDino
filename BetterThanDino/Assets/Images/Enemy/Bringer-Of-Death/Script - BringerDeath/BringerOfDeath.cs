using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeath : MonoBehaviour
{
    public float walkSpeed = 2f; // Walking speed
    public LayerMask playerLayer; // Layer to detect the player
    public float detectionRadius = 3f; // Radius to detect the player
    private Animator animator;
    private Rigidbody2D rb;
    private bool isAttacking = false; // To prevent redundant attacks
    private float spawnTime;

    // Pre-allocate an array for OverlapCircleNonAlloc
    private Collider2D[] detectedColliders = new Collider2D[1];

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spawnTime = Time.time;

        // Start walking immediately
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        // Check if the enemy should die
        if (Time.time - spawnTime >= 10f)
        {
            Die();
            return;
        }

        // If not attacking, keep walking
        if (!isAttacking)
        {
            WalkLeft();
        }

        // Check for player detection using OverlapCircleNonAlloc
        int detectedCount = Physics2D.OverlapCircleNonAlloc(transform.position, detectionRadius, detectedColliders, playerLayer);

        if (detectedCount > 0 && !isAttacking)
        {
            Attack();
        }
    }

    void WalkLeft()
    {
        // Move the enemy to the left
        rb.velocity = new Vector2(-walkSpeed, rb.velocity.y);
        animator.SetBool("isWalking", true);
    }

    void Attack()
    {
        // Stop moving and play attack animation
        isAttacking = true;
        rb.velocity = Vector2.zero; // Stop movement
        animator.SetBool("isWalking", false);
        animator.SetTrigger("Attack");
    }

    void Die()
    {
        // Stop moving and play death animation
        rb.velocity = Vector2.zero; // Stop movement
        animator.SetTrigger("Die");
        Destroy(gameObject, 1.5f); // Destroy the GameObject after the death animation plays
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the detection radius in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
