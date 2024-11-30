using UnityEngine;

public class KnightCombatSensor : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private Animator animator; // Animator for the knight
    [SerializeField] private float attackCooldown = 2f; // Cooldown between attacks
    private float lastAttackTime; // Tracks the last attack time

    [Header("Sensor Settings")]
    [SerializeField] private Transform sensorPosition; // Position of the sensor
    [SerializeField] private float sensorRadius = 2f; // Radius of the sensor
    [SerializeField] private LayerMask enemyLayer; // Layer for detecting enemies

    private void Update()
    {
        // Check for enemies within sensor range
        Collider2D[] enemies = Physics2D.OverlapCircleAll(sensorPosition.position, sensorRadius, enemyLayer);

        // If an enemy is detected and cooldown has passed, attack
        if (enemies.Length > 0 && Time.time >= lastAttackTime + attackCooldown)
        {
            PerformRandomAttack();
            lastAttackTime = Time.time; // Reset the cooldown timer
        }
    }

    private void PerformRandomAttack()
    {
        // Randomize between Attack1, Attack2, and Attack3
        int randomAttack = Random.Range(1, 4); // Generates 1, 2, or 3

        // Trigger the corresponding attack animation
        animator.SetTrigger($"Attack{randomAttack}");

        Debug.Log($"Performed Attack{randomAttack}");
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the sensor radius for debugging
        if (sensorPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(sensorPosition.position, sensorRadius);
        }
    }
}