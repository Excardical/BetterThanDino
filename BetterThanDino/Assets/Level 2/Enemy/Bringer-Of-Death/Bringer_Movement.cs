using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bringer_Movement : MonoBehaviour
{
    public float speed;
    public float attackRange = 2;
    public float attackCooldown = 2;
    public float playerDetectRange = 5;
    public float postAttackPauseTime = 100; // Reduced to a more reasonable time

    public Transform detectionPoint;
    public LayerMask playerLayer;

    private float attackCooldownTimer;
    private BringerState bringerState; // Updated keyword
    private bool isPostAttackPausing = false;

    private Rigidbody2D rb;
    private Transform target;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        MoveLeft();
    }

    void Update()
    {
        if (bringerState != BringerState.Knockback) // Updated keyword
        {
            if (attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime;
            }

            switch (bringerState) // Updated keyword
            {
                case BringerState.Idle: // Updated keyword
                    rb.velocity = Vector2.zero;
                    break;
                case BringerState.Chasing: // Updated keyword
                    Chase();
                    break;
                case BringerState.Attacking: // Updated keyword
                    rb.velocity = Vector2.zero;
                    break;
                case BringerState.MoveLeft: // Updated keyword
                    MoveLeft();
                    break;
            }

            CheckForPlayer();
        }
    }

    void MoveLeft()
    {
        rb.velocity = Vector2.left * speed;
        ChangeState(BringerState.MoveLeft); // Updated keyword
    }

    void Chase()
    {
        Vector2 direction = new Vector2(target.position.x - transform.position.x, 0).normalized;
        rb.velocity = direction * speed;
    }

    private void CheckForPlayer()
    {
        if (isPostAttackPausing) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);

        if (hits.Length > 0)
        {
            target = hits[0].transform;

            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            if (distanceToTarget <= attackRange && attackCooldownTimer <= 0)
            {
                ChangeState(BringerState.Attacking); // Updated keyword
                attackCooldownTimer = attackCooldown;
                StartCoroutine(PostAttackPause());
            }
            else if (distanceToTarget > attackRange && bringerState != BringerState.Attacking) // Updated keyword
            {
                ChangeState(BringerState.Chasing); // Updated keyword
            }
        }
        else if (bringerState != BringerState.Attacking) // Updated keyword
        {
            ChangeState(BringerState.MoveLeft); // Updated keyword
        }
    }

    private IEnumerator PostAttackPause()
    {
        isPostAttackPausing = true;

        // Play the attack animation and wait for it to complete
        float attackAnimationLength = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(attackAnimationLength);

        // Transition to Idle state and pause
        ChangeState(BringerState.Idle); // Updated keyword
        yield return new WaitForSeconds(postAttackPauseTime);
        isPostAttackPausing = false;
    }

    public void ChangeState(BringerState newState) // Updated keyword
    {
        // Reset all animation states
        anim.SetBool("isIdle", false);
        anim.SetBool("isChasing", false);
        anim.SetBool("isAttacking", false);

        bringerState = newState; // Updated keyword

        // Set the appropriate animation state
        switch (newState) // Updated keyword
        {
            case BringerState.Idle: // Updated keyword
                anim.SetBool("isIdle", true);
                break;
            case BringerState.Chasing: // Updated keyword
                anim.SetBool("isChasing", true);
                break;
            case BringerState.Attacking: // Updated keyword
                anim.SetBool("isAttacking", true);
                break;
            case BringerState.MoveLeft: // Updated keyword
                anim.SetBool("isChasing", true);
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint.position, playerDetectRange);
    }
}

public enum BringerState // Updated keyword
{
    Idle,
    Chasing,
    Attacking,
    Knockback,
    MoveLeft
}
