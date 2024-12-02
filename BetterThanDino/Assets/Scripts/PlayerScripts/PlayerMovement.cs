using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce; // Force applied to jump
    private bool isGrounded; // To check if the player is on the ground
    private bool isKnockedBack;
    public Player_Combat player_Combat;
    public int facingDirection = 1;
    private Rigidbody2D body;
    public Animator anim;
    [SerializeField] private Transform groundCheck; // Transform to check ground collision
    [SerializeField] private float groundCheckRadius; // Radius of the ground check
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (isKnockedBack == false)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            body.velocity = new Vector2(horizontalInput * StatsManager.Instance.speed, body.velocity.y);

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            if (horizontalInput > 0 && transform.localScale.x < 0 ||
                horizontalInput < 0 && transform.localScale.x > 0)
            {
                Flip();
            }
            anim.SetFloat("horizontal", Mathf.Abs(horizontalInput));
        }
    }
    private void Update()
    {
        if(Input.GetButtonDown("Slash"))
        {
            player_Combat.Attack();
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        //to jump    
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }
    }


    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void Knockback(Transform enemy, float force, float stunTime)
    {
        isKnockedBack = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        body.velocity = direction * force;
        StartCoroutine(KnockbackCounter(stunTime));
    }

    IEnumerator KnockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        body.velocity = Vector2.zero;
        isKnockedBack = false;
    }
}
