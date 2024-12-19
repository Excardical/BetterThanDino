using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;      // Maximum health of the enemy
    private int currentHealth;       // Current health
    private Animator animator;       // Reference to the Animator component
    private bool isDead = false;     // To prevent multiple triggers

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator not found on " + gameObject.name);
        }
    }

    // Function to apply damage to the enemy
    public void TakeDamage(int damage)
    {
        if (isDead) return;  // Ignore damage if already dead

        currentHealth -= damage;
        Debug.Log($"Enemy Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Handles the death of the enemy
    private void Die()
    {
        isDead = true;  // Prevent multiple death triggers

        // Set the "Death" parameter in the Animator to true
        if (animator != null)
        {
            animator.SetBool("Death", true);
        }

        // Optionally disable enemy behaviors, colliders, etc.
        GetComponent<Collider2D>().enabled = false; // For 2D, replace with Collider for 3D
        this.enabled = false;  // Disable this script after death
    }
}