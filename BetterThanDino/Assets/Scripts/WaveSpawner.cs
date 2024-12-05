using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public float spawnRate = 1.0f;
    public float timeBetweenWaves = 3.0f;

    public int enemyCount;
    bool waveIsDone = true;
    public GameObject enemy;

    void Update()
    {
        if(waveIsDone)
            StartCoroutine(waveSpawner());
    }

    IEnumerator waveSpawner()
    {
        waveIsDone = false;
        for(int i = 0; i < enemyCount; i++)
        {
            GameObject enemyClone = Instantiate(enemy, new Vector2(20, -2), Quaternion.identity);

            yield return new WaitForSeconds(spawnRate);
        }

        spawnRate -= 0.1f;
        enemyCount += 3;

        yield return new WaitForSeconds(timeBetweenWaves);

        waveIsDone = true;
    }
}
