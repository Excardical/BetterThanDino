using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    public int damage = 1;
    public Transform attackPoint;
    public float weaponRange;
    public float knockbackForce;
    public float stunTime;
    public LayerMask playerLayer;

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.tag == "Player")
    //         collision.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
    // }

    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);

        if (hits.Length > 0)
        {
            // Process the first hit
            PlayerHealth playerHealth = hits[0].GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(-damage);
            }

            PlayerMovement playerMovement = hits[0].GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.Knockback(transform, knockbackForce, stunTime);
            }
        }
        else
        {
            Debug.Log("No player detected in the attack range.");
        }
    }
}
