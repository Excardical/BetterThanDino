using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D body;

    public float Health, MaxHealth;
    [SerializeField] private HealthbarUI healthBar;

    private void Awake()
    {
        healthBar.SetMaxHealth(MaxHealth);
        body = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
    }
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        //flip player when moving left/right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(1, -1, 1);
        
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
