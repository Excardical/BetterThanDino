using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class EnemySpawnData
{
    [Tooltip("Type of enemy to spawn")]
    public GameObject enemyPrefab;
    
    [Tooltip("Number of enemies to spawn")]
    public int enemyCount;
}

[Serializable]
public class WaveMilestone
{
    [Tooltip("Progress percentage for this milestone")]
    [Range(0f, 100f)]
    public float triggerPercentage;

    [Tooltip("Enemies to spawn at this milestone")]
    public EnemySpawnData[] enemySpawns;
}

public class WaveManager : MonoBehaviour
{
    [Header("Wave Progression Settings")]
    public float totalWaveDuration = 30f;
    public float spawnRate = 0.5f;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Wave Progress")]
    [SerializeField] private Slider waveProgressSlider;
    [SerializeField] private Gradient sliderColorGradient;

    [Header("Wave Milestones")]
    public List<WaveMilestone> waveMilestones = new List<WaveMilestone>();

    // Wave state variables
    private float currentTime;
    private bool isWaveActive = false;
    private bool isWaveComplete = false;
    private bool isSpawningEnemies = false;
    private List<int> triggeredMilestones = new List<int>();
    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool waitingForSpawn = false;

    void Start()
    {
        if (waveProgressSlider == null)
        {
            waveProgressSlider = GetComponent<Slider>();
        }

        if (waveProgressSlider != null)
        {
            waveProgressSlider.minValue = 0;
            waveProgressSlider.maxValue = 100;
            waveProgressSlider.value = 0;
        }

        waveMilestones.Sort((a, b) => a.triggerPercentage.CompareTo(b.triggerPercentage));
        StartWaveProgression();
    }

    void Update()
    {
        if (isWaveActive && !isWaveComplete && !waitingForSpawn)
        {
            UpdateWaveProgression();
        }
    }

    private bool AreAllEnemiesCleared()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
        return activeEnemies.Count == 0;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }

    void StartWaveProgression()
    {
        currentTime = 0;
        isWaveActive = true;
        isWaveComplete = false;
        isSpawningEnemies = false;
        waitingForSpawn = false;
        triggeredMilestones.Clear();
        activeEnemies.Clear();
    }

    void UpdateWaveProgression()
    {
        if (waveProgressSlider != null)
        {
            currentTime += Time.deltaTime;
            float progressPercentage = (currentTime / totalWaveDuration) * 100f;
            waveProgressSlider.value = progressPercentage;

            if (sliderColorGradient != null)
            {
                float normalizedTime = currentTime / totalWaveDuration;
                waveProgressSlider.fillRect.GetComponent<Image>().color = 
                    sliderColorGradient.Evaluate(normalizedTime);
            }

            CheckMilestoneSpawns(progressPercentage);

            if (progressPercentage >= 100 && !isWaveComplete)
            {
                isWaveComplete = true;
                isWaveActive = false;
                OnWaveComplete();
            }
        }
    }

    void CheckMilestoneSpawns(float progressPercentage)
    {
        for (int i = 0; i < waveMilestones.Count; i++)
        {
            if (!triggeredMilestones.Contains(i) && progressPercentage >= waveMilestones[i].triggerPercentage)
            {
                waitingForSpawn = true;  // Pause the slider
                SpawnMilestoneEnemies(waveMilestones[i]);
                triggeredMilestones.Add(i);
                break;  // Only process one milestone at a time
            }
        }
    }

    void SpawnMilestoneEnemies(WaveMilestone milestone)
    {
        StartCoroutine(SpawnEnemiesRoutine(milestone.enemySpawns));
    }

    IEnumerator SpawnEnemiesRoutine(EnemySpawnData[] enemySpawns)
    {
        isSpawningEnemies = true;

        foreach (var enemySpawn in enemySpawns)
        {
            for (int i = 0; i < enemySpawn.enemyCount; i++)
            {
                if (spawnPoints.Length > 0 && enemySpawn.enemyPrefab != null)
                {
                    Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
                    GameObject enemy = Instantiate(enemySpawn.enemyPrefab, spawnPoint.position, Quaternion.identity);
                    activeEnemies.Add(enemy);
                }
                else if (enemySpawn.enemyPrefab != null)
                {
                    GameObject enemy = Instantiate(enemySpawn.enemyPrefab, new Vector2(20, -2), Quaternion.identity);
                    activeEnemies.Add(enemy);
                }

                yield return new WaitForSeconds(spawnRate);
            }
        }

        isSpawningEnemies = false;
        waitingForSpawn = false;  // Resume the slider after spawning is complete
    }

    void OnWaveComplete()
    {
        Debug.Log("Wave Complete!");
    }

    public void PauseWave()
    {
        isWaveActive = false;
    }

    public void ResumeWave()
    {
        if (!isWaveComplete)
        {
            isWaveActive = true;
        }
    }

    public void RestartWave()
    {
        StartWaveProgression();
    }

    public void SetWaveDuration(float duration)
    {
        totalWaveDuration = duration;
    }
}