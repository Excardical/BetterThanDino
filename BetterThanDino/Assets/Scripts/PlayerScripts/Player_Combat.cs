using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public Animator anim;
    private float timer;
    
    public AudioClip attackSound; // Attack sound effect
    private AudioSource audioSource;

    private void Start()
    {
        // Set up the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        if (timer <= 0)
        {
            anim.SetBool("isAttacking", true);
            // Play the attack sound
            PlayAttackSound();
            timer = StatsManager.Instance.cooldown;
        }
    }

    public void DealDamage()
    {        
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, StatsManager.Instance.weaponRange, enemyLayer);

        foreach (var enemyCollider in enemies)
        {
            EnemyBase enemy = enemyCollider.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                // Apply damage
                enemy.TakeDamage(StatsManager.Instance.damage);

                // Apply knockback
                enemy.ApplyKnockback(transform);
            }
        }
    }

    public void FinishAttacking()
    {
        anim.SetBool("isAttacking", false);
    }

    private void PlayAttackSound()
    {
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, StatsManager.Instance.weaponRange);
    }
}
