using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFireball : MonoBehaviour
{
    public float speed = 5f; // Fireball move speed
    public int damage = 1;
    private Vector3 direction; // Fire falling direction
    
    // Start is called before the first frame update
    void Start()
    {
        // Fixed direction of positive 45° drop
        direction = Quaternion.Euler(0, 0, -45f) * Vector3.down;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the fireball in the direction
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("The player is hit!");
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(-damage);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("The enemy is hit!");
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("The fireball hit the ground!");
            Destroy(gameObject);
        }
    }
}
