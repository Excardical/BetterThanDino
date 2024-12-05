using UnityEngine;

public class FallingFireball : MonoBehaviour
{
    public float speed = 5f; // Fireball move speed

    private Vector3 direction;

    void Start()
    {
        // Fixed direction of positive 45° drop
        direction = Quaternion.Euler(0, 0, -45f) * Vector3.down;
    }

    void Update()
    {
        // Move the fireball in the direction
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Touching the player's logic
            Debug.Log("The player is hit!");
        }

        // Destruction of fireballs after collision
        Destroy(gameObject);
    }
}
