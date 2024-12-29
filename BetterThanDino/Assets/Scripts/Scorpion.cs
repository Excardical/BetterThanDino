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
    [SerializeField] private LayerMask baseLayer;
    private Animator anim;
    private PlayerHealth playerHealth;
    private BaseHealth baseHealth;
    private ScorpioPatrol enemyPatrol;

    [SerializeField] private AudioSource scorpioAttackSound; // Single AudioSource for attack sound

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

        if (PlayerInSight() || BaseInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !(PlayerInSight() || BaseInSight());
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

    private bool BaseInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.right, 0, baseLayer);

        if (hit.collider != null)
        {
            baseHealth = hit.transform.GetComponent<BaseHealth>();
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
            playerHealth.ChangeHealth(-damage);
        }
        else if (baseHealth != null && baseHealth.gameObject.activeInHierarchy)
        {
            baseHealth.ChangeHealth(-damage);
        }

        // Play the scorpion attack sound
        if (scorpioAttackSound != null)
        {
            scorpioAttackSound.Play();
        }
    }
}
