using UnityEngine;

public class KnightScriptTest : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f; 
    private bool isGrounded; 
    private Rigidbody2D body;

    [Header("Animator Settings")]
    [SerializeField] private Animator animator;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck; 
    [SerializeField] private float groundCheckRadius = 0.2f; 
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Movement logic
        float horizontalInput = Input.GetAxis("Horizontal");

        // Set Rigidbody velocity for movement
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // Update the Speed parameter for the Animator
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));

        // Ground check logic
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Update()
    {
        // Flip character based on movement direction
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one; // Face right
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1); // Face left

        // Jumping logic
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            animator.SetBool("IsGrounded", false); // Jump animation
        }

        // Update grounded state
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize ground check radius for debugging
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
