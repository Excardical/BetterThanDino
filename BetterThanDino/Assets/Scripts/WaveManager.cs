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
    public float spawnRate = 1.0f;

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
            waveProgressSlider.maxValue = 100;
            waveProgressSlider.value = 0;
        }

        // Sort milestones by trigger percentage
        waveMilestones.Sort((a, b) => a.triggerPercentage.CompareTo(b.triggerPercentage));

        // Start the wave progression
        StartWaveProgression();
    }

    void Update()
    {
        if (isWaveActive)
        {
            UpdateWaveProgression();
        }
    }

    void StartWaveProgression()
    {
        currentTime = 0;
        isWaveActive = true;
        triggeredMilestones.Clear();
    }

    void UpdateWaveProgression()
    {
        if (waveProgressSlider != null)
        {
            // Increment time
            currentTime += Time.deltaTime;

            // Calculate progress percentage
            float progressPercentage = (currentTime / totalWaveDuration) * 100f;
            waveProgressSlider.value = progressPercentage;

            // Update slider color
            if (sliderColorGradient != null)
            {
                float normalizedTime = currentTime / totalWaveDuration;
                waveProgressSlider.fillRect.GetComponent<Image>().color = 
                    sliderColorGradient.Evaluate(normalizedTime);
            }

            // Check and spawn enemies at milestones
            CheckMilestoneSpawns(progressPercentage);

            // Reset wave when complete
            if (progressPercentage >= 100)
            {
                isWaveActive = false;
                StartWaveProgression();
            }
        }
    }

    void CheckMilestoneSpawns(float progressPercentage)
    {
        for (int i = 0; i < waveMilestones.Count; i++)
        {
            // Check if this milestone hasn't been triggered yet and current progress meets or exceeds milestone
            if (!triggeredMilestones.Contains(i) && progressPercentage >= waveMilestones[i].triggerPercentage)
            {
                SpawnMilestoneEnemies(waveMilestones[i]);
                triggeredMilestones.Add(i);
            }
        }
    }

    void SpawnMilestoneEnemies(WaveMilestone milestone)
    {
        StartCoroutine(SpawnEnemiesRoutine(milestone.enemySpawns));
    }

    IEnumerator SpawnEnemiesRoutine(EnemySpawnData[] enemySpawns)
    {
        foreach (var enemySpawn in enemySpawns)
        {
            for (int i = 0; i < enemySpawn.enemyCount; i++)
            {
                // Spawn enemies at random spawn points
                if (spawnPoints.Length > 0 && enemySpawn.enemyPrefab != null)
                {
                    // Explicitly use UnityEngine.Random
                    Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
                    Instantiate(enemySpawn.enemyPrefab, spawnPoint.position, Quaternion.identity);
                }
                else if (enemySpawn.enemyPrefab != null)
                {
                    // Fallback to default spawn if no points defined
                    Instantiate(enemySpawn.enemyPrefab, new Vector2(20, -2), Quaternion.identity);
                }

                yield return new WaitForSeconds(spawnRate);
            }
        }
    }

    // Optional methods for additional control
    public void PauseWave()
    {
        isWaveActive = false;
    }

    public void ResumeWave()
    {
        isWaveActive = true;
    }

    public void SetWaveDuration(float duration)
    {
        totalWaveDuration = duration;
    }
}