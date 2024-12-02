using UnityEngine;

public class FireballGenerate : MonoBehaviour
{
    public GameObject fireballPrefab; 
    public float spawnInterval = 2f; // Generation interval
    public float spawnRange = 10f; // Range of generated positions

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnFireball();
            timer = 0;
        }
    }

    void SpawnFireball()
    {
        // Randomly generate the X position of the fireball
        float randomX = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, 0);

        // Generate fireballs
        Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);
    }
}
