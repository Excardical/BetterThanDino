using UnityEngine;

public class Scorpion : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float colliderDistance;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask baseLayer; // **New**: Reference to the layer of the base
    private Animator anim;
    private PlayerHealth playerHealth;
    private BaseHealth baseHealth; // **New**: Reference to the BaseHealth script
    private ScorpioPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponent<ScorpioPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        playerHealth = null;
        baseHealth = null;

        // Player detection and attack
        if (PlayerInSight() || BaseInSight()) // Attack player or base
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !(PlayerInSight() || BaseInSight()); // **New**: Disable patrol if attacking player or base
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.right, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<PlayerHealth>();
        }

        return hit.collider != null;
    }

    // **New**: Base detection logic (similar to PlayerInSight)
    private bool BaseInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.right, 0, baseLayer); // Use the base layer

        if (hit.collider != null)
        {
            baseHealth = hit.transform.GetComponent<BaseHealth>(); // Get the BaseHealth component
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (playerHealth != null && playerHealth.gameObject.activeInHierarchy)
        {
            playerHealth.ChangeHealth(-damage); // Apply damage to the player
        }
        else if (baseHealth != null && baseHealth.gameObject.activeInHierarchy)
        {
            baseHealth.ChangeHealth(-damage); // Apply damage to the base
        }
    }
}
