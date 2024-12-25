using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatLVL3 : MonoBehaviour
{
    public Transform attackPoint; // The position where the attack originates
    public LayerMask enemyLayer; // Layer that identifies enemies
    public Animator anim; // Reference to the Animator for triggering animations
    private float timer; // Tracks cooldown time

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        // Handle attack input (left-click)
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (timer <= 0)
        {
            anim.SetBool("isAttacking", true); // Trigger attack animation
            timer = StatsManager.Instance.cooldown; // Reset the cooldown
        }
    }

    public void DealDamage()
    {
        // Detect enemies within the attack range
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, StatsManager.Instance.weaponRange, enemyLayer);

        foreach (var enemyCollider in enemies)
        {
            Health enemyHealth = enemyCollider.GetComponent<Health>();
            if (enemyHealth != null)
            {
                // Apply damage to the enemy
                enemyHealth.TakeDamage(StatsManager.Instance.damage);

                Debug.Log($"Dealt {StatsManager.Instance.damage} damage to {enemyHealth.name}");
            }
        }
    }

    public void FinishAttacking()
    {
        anim.SetBool("isAttacking", false); // End attack animation
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, StatsManager.Instance.weaponRange);
    }
}
