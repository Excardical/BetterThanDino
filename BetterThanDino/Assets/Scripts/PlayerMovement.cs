using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce; // Force applied to jump
    private bool isGrounded; // To check if the player is on the ground
    private Rigidbody2D body;

    public float Health, MaxHealth;
    [SerializeField] private HealthbarUI healthBar;
    [SerializeField] private Transform groundCheck; // Transform to check ground collision
    [SerializeField] private float groundCheckRadius; // Radius of the ground check
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        healthBar.SetMaxHealth(MaxHealth);
        body = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        //flip player when moving left/right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(1, -1, 1);
            
        //to jump    
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }

        if(Input.GetKeyDown("x")) {
            SetHealth(-20f);
        }
        if(Input.GetKeyDown("z")) {
            SetHealth(+20f);
        }
    }

    public void SetHealth(float healthChange) {
        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        healthBar.SetHealth(Health);
    }
}
