using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce; // Force applied to jump
    private bool isGrounded; // To check if the player is on the ground
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
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (horizontalInput > 0 && transform.localScale.x < 0 ||
            horizontalInput < 0 && transform.localScale.x > 0)
        {
            Flip();
        }
        anim.SetFloat("horizontal", Mathf.Abs(horizontalInput));
    }
    private void Update()
    {
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
        transform.localScale = new Vector3(transform.localScale.x*-1,transform.localScale.y,transform.localScale.z);
    }
}
