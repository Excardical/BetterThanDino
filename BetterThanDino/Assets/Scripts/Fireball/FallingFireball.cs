using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFireball : MonoBehaviour
{
    public float speed = 5f; // Fireball move speed
    public int damage = 1;
    public float stunDuration = 1f; // 静止持续时间
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
            StatsManager statsManager = collision.gameObject.GetComponent<StatsManager>();
            Animator animator = collision.gameObject.GetComponent<Animator>();

            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(-damage);
            }

            if (statsManager != null && animator != null)
            {
                // 创建一个空的游戏对象来执行协程
                GameObject stunHandler = new GameObject("StunHandler");
                StunCoroutineHandler handler = stunHandler.AddComponent<StunCoroutineHandler>();
                handler.StartStun(statsManager, animator, stunDuration);
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

    public class StunCoroutineHandler : MonoBehaviour
    {
        public void StartStun(StatsManager statsManager, Animator animator, float duration)
        {
            StartCoroutine(StunCoroutine(statsManager, animator, duration));
        }

        private IEnumerator StunCoroutine(StatsManager statsManager, Animator animator, float duration)
       {
          // 保存原始速度（整数）
            int originalSpeed = statsManager.speed;
            statsManager.speed = 0;
            animator.speed = 0;

            yield return new WaitForSeconds(duration);

            if (statsManager != null)
            {
                // 恢复原始速度（整数）
                statsManager.speed = originalSpeed;
                animator.speed = 1;
            }

            // 协程完成后销毁这个处理器对象
            Destroy(gameObject);
        }
    }
}
