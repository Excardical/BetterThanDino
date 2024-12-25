using UnityEngine;

public class Scorpion : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float colliderDistance ;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    private Animator anim;
    private PlayerHealth playerHealth;
    private ScorpioPatrol enemyPatrol;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<ScorpioPatrol>();
    }
    private void Update()
    {
        cooldownTimer+=Time.deltaTime;
        if(PlayerInSight())
        {
            if(cooldownTimer>=attackCooldown)
            {
                cooldownTimer=0;
                anim.SetTrigger("meleeAttack");
            }
        }
        if(enemyPatrol!=null)
        {
            enemyPatrol.enabled=!PlayerInSight();
        }
    }
    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right*range*transform.localScale.x*colliderDistance,
        new Vector3(boxCollider.bounds.size.x*range,boxCollider.bounds.size.y,boxCollider.bounds.size.z),
        0,Vector2.left,0,playerLayer);
        if(hit.collider!=null)
            playerHealth = hit.transform.GetComponent<PlayerHealth>();
        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right*range*transform.localScale.x*colliderDistance, 
        new Vector3(boxCollider.bounds.size.x*range,boxCollider.bounds.size.y,boxCollider.bounds.size.z));

    }
    private void DamagePlayer()
   {
        if(PlayerInSight())
        {
            playerHealth.ChangeHealth(-damage);
        }
    }
}
