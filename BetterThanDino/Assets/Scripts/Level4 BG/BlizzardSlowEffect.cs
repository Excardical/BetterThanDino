using UnityEngine;

public class BlizzardSlowEffect : MonoBehaviour
{
    public float slowFactor = 0.75f;
    private int originalSpeed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            originalSpeed = StatsManager.Instance.speed;

            StatsManager.Instance.speed = (int)(StatsManager.Instance.speed * slowFactor);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StatsManager.Instance.speed = originalSpeed;
        }
    }
}
