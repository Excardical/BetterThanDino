// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class Enemy_Movement : MonoBehaviour
// {
//     public float speed;
//     public float attackRange = 2;
//     public float attackCooldown = 2;
//     public float playerDetectRange = 5;
//     public float postAttackPauseTime = 100; // Reduced to a more reasonable time
//
//     public Transform detectionPoint;
//     public LayerMask playerLayer;
//
//     private float attackCooldownTimer;
//     private EnemyState enemyState;
//     private bool isPostAttackPausing = false;
//
//     private Rigidbody2D rb;
//     private Transform target;
//     private Animator anim;
//
//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         anim = GetComponent<Animator>();
//         MoveLeft();
//     }
//
//     void Update()
//     {
//         if (enemyState != EnemyState.Knockback)
//         {
//             if (attackCooldownTimer > 0)
//             {
//                 attackCooldownTimer -= Time.deltaTime;
//             }
//
//             switch (enemyState)
//             {
//                 case EnemyState.Idle:
//                     rb.velocity = Vector2.zero;
//                     break;
//                 case EnemyState.Chasing:
//                     Chase();
//                     break;
//                 case EnemyState.Attacking:
//                     
//                     
//                     
//                     rb.velocity = Vector2.zero;
//                     break;
//                 case EnemyState.MoveLeft:
//                     MoveLeft();
//                     break;
//             }
//
//             CheckForPlayer();
//         }
//     }
//
//     void MoveLeft()
//     {
//         rb.velocity = Vector2.left * speed;
//         ChangeState(EnemyState.MoveLeft);
//     }
//
//     void Chase()
//     {
//         Vector2 direction = new Vector2(target.position.x - transform.position.x, 0).normalized;
//         rb.velocity = direction * speed;
//     }
//
//     private void CheckForPlayer()
//     {
//         if (isPostAttackPausing) return;
//
//         Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);
//
//         if (hits.Length > 0)
//         {
//             target = hits[0].transform;
//
//             float distanceToTarget = Vector2.Distance(transform.position, target.position);
//
//             if (distanceToTarget <= attackRange && attackCooldownTimer <= 0)
//             {
//                 ChangeState(EnemyState.Attacking);
//                 attackCooldownTimer = attackCooldown;
//                 StartCoroutine(PostAttackPause());
//             }
//             else if (distanceToTarget > attackRange && enemyState != EnemyState.Attacking)
//             {
//                 ChangeState(EnemyState.Chasing);
//             }
//         }
//         else if (enemyState != EnemyState.Attacking)
//         {
//             ChangeState(EnemyState.MoveLeft);
//         }
//     }
//
//
//     private IEnumerator PostAttackPause()
//     {
//         isPostAttackPausing = true;
//
//         // Play the attack animation and wait for it to complete
//         float attackAnimationLength = anim.GetCurrentAnimatorStateInfo(0).length;
//         yield return new WaitForSeconds(attackAnimationLength);
//
//         // Transition to Idle state and pause
//         ChangeState(EnemyState.Idle);
//         yield return new WaitForSeconds(postAttackPauseTime);
//         isPostAttackPausing = false;
//     }
//
//
//     public void ChangeState(EnemyState newState)
//     {
//         // Reset all animation states
//         anim.SetBool("isIdle", false);
//         anim.SetBool("isChasing", false);
//         anim.SetBool("isAttacking", false);
//
//         enemyState = newState;
//
//         // Set the appropriate animation state
//         switch (newState)
//         {
//             case EnemyState.Idle:
//                 anim.SetBool("isIdle", true);
//                 break;
//             case EnemyState.Chasing:
//                 anim.SetBool("isChasing", true);
//                 break;
//             case EnemyState.Attacking:
//                 anim.SetBool("isAttacking", true);
//                 break;
//             case EnemyState.MoveLeft:
//                 anim.SetBool("isChasing", true);
//                 break;
//         }
//     }
//
//     private void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(detectionPoint.position, playerDetectRange);
//     }
// }
//
// public enum EnemyState
// {
//     Idle,
//     Chasing,
//     Attacking,
//     Knockback,
//     MoveLeft
// }