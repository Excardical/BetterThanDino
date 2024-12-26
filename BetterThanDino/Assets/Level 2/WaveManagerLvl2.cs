using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class EnemySpawnData2
{
    [Tooltip("Type of enemy to spawn")]
    public GameObject enemyPrefab;
    
    [Tooltip("Number of enemies to spawn")]
    public int enemyCount;
}

[Serializable]
public class WaveMilestone2
{
    [Tooltip("Spawn time for this milestone in seconds")]
    public float spawnTime;

    [Tooltip("Enemies to spawn at this milestone")]
    public EnemySpawnData2[] enemySpawns;
}

public class WaveManagerLvl2 : MonoBehaviour
{
    [Header("Wave Progression Settings")]
    public float totalWaveDuration = 30f;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Wave Progress")]
    [SerializeField] private Slider waveProgressSlider;
    [SerializeField] private Gradient sliderColorGradient;

    [Header("Wave Milestones")]
    public List<WaveMilestone2> waveMilestones = new List<WaveMilestone2>();

    // Wave state variables
    private float currentTime;
    private bool isWaveActive = false;
    private bool isWaveComplete = false;
    private List<int> triggeredMilestones = new List<int>();

    void Start()
    {
        // Ensure slider is set up correctly
        if (waveProgressSlider == null)
        {
            waveProgressSlider = GetComponent<Slider>();
        }

        // Initialize slider
        if (waveProgressSlider != null)
        {
            waveProgressSlider.minValue = 0;
            waveProgressSlider.maxValue = totalWaveDuration;
            waveProgressSlider.value = 0;
        }

        // Sort milestones by spawn time
        waveMilestones.Sort((a, b) => a.spawnTime.CompareTo(b.spawnTime));

        // Start the wave progression
        StartWaveProgression();
    }

    void Update()
    {
        if (isWaveActive && !isWaveComplete)
        {
            UpdateWaveProgression();
        }
    }

    void StartWaveProgression()
    {
        currentTime = 0;
        isWaveActive = true;
        isWaveComplete = false;
        triggeredMilestones.Clear();
    }

    void UpdateWaveProgression()
    {
        if (waveProgressSlider != null)
        {
            // Increment time
            currentTime += Time.deltaTime;

            // Update slider
            waveProgressSlider.value = currentTime;

            // Update slider color
            if (sliderColorGradient != null)
            {
                float normalizedTime = currentTime / totalWaveDuration;
                waveProgressSlider.fillRect.GetComponent<Image>().color = 
                    sliderColorGradient.Evaluate(normalizedTime);
            }

            // Check and spawn enemies at milestones
            CheckMilestoneSpawns(currentTime);

            // Mark wave as complete when reaching the total duration
            if (currentTime >= totalWaveDuration && !isWaveComplete)
            {
                isWaveComplete = true;
                isWaveActive = false;
                OnWaveComplete();
            }
        }
    }

    void CheckMilestoneSpawns(float elapsedTime)
    {
        for (int i = 0; i < waveMilestones.Count; i++)
        {
            // Check if this milestone hasn't been triggered yet and elapsed time meets the milestone time
            if (!triggeredMilestones.Contains(i) && elapsedTime >= waveMilestones[i].spawnTime)
            {
                SpawnMilestoneEnemies(waveMilestones[i]);
                triggeredMilestones.Add(i);
            }
        }
    }

    void SpawnMilestoneEnemies(WaveMilestone2 milestone)
    {
        StartCoroutine(SpawnEnemiesRoutine(milestone.enemySpawns));
    }

    IEnumerator SpawnEnemiesRoutine(EnemySpawnData2[] enemySpawns)
    {
        foreach (var enemySpawn in enemySpawns)
        {
            for (int i = 0; i < enemySpawn.enemyCount; i++)
            {
                // Spawn enemies at random spawn points
                if (spawnPoints.Length > 0 && enemySpawn.enemyPrefab != null)
                {
                    Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
                    Instantiate(enemySpawn.enemyPrefab, spawnPoint.position, Quaternion.identity);
                }
                else if (enemySpawn.enemyPrefab != null)
                {
                    // Fallback to default spawn if no points defined
                    Instantiate(enemySpawn.enemyPrefab, new Vector2(20, -2), Quaternion.identity);
                }

                // Add a randomized delay between 1 to 4 seconds
                float randomDelay = UnityEngine.Random.Range(1f, 4f);
                yield return new WaitForSeconds(randomDelay);
            }
        }
    }



    void OnWaveComplete()
    {
        // You can add any wave completion logic here
        Debug.Log("Wave Complete!");
    }

    // Optional methods for additional control
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
