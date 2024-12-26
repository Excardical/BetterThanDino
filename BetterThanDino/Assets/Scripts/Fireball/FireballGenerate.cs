using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballGenerate : MonoBehaviour
{
    public GameObject fireballPrefab;
    public float spawnInterval = 1f; // Generation interval
    public float spawnRange = 5f; // Range of generated

    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Update timer per frame
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnFireball(); // Generate Fireball
            timer = 0; // Reset timer
        }
    }

    void SpawnFireball()
    {
        if (fireballPrefab == null)
        {
            Debug.LogError("Fireball prefab is not assigned!");
            return;
        }

        // Randomly generate the X position of the fireball
        float randomX = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPosition = new Vector3(transform.position.x + randomX, transform.position.y, 0);

        // Generate fireballs
        Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);
    }
}
