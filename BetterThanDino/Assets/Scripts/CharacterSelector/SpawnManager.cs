using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [Header("Spawn UI")]
    [SerializeField] private GameObject spawnButtonPrefab;
    [SerializeField] private Transform spawnButtonContainer;
    
    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPointsParent; // Parent object containing spawn point transforms
    private Transform[] spawnPoints;
    private int currentSpawnPointIndex = 0;

    private List<SpawnButton> spawnButtons = new List<SpawnButton>();

    private void Awake()
    {
        Instance = this;

        // Debug to ensure references are set
        if (spawnButtonContainer == null)
            Debug.LogError("SpawnManager: 'spawnButtonContainer' is not assigned in the Inspector!");

        if (spawnButtonPrefab == null)
            Debug.LogError("SpawnManager: 'spawnButtonPrefab' is not assigned in the Inspector!");
            
        if (spawnPointsParent == null)
            Debug.LogError("SpawnManager: 'spawnPointsParent' is not assigned in the Inspector!");
        else
            InitializeSpawnPoints();
    }

    private void InitializeSpawnPoints()
    {
        // Get all child transforms from the spawn points parent
        spawnPoints = new Transform[spawnPointsParent.childCount];
        for (int i = 0; i < spawnPointsParent.childCount; i++)
        {
            spawnPoints[i] = spawnPointsParent.GetChild(i);
        }
        
        if (spawnPoints.Length == 0)
            Debug.LogError("SpawnManager: No spawn points found under the spawn points parent!");
    }

    public void Initialize(List<CharacterData> characters)
    {
        if (spawnButtonContainer == null || spawnButtonPrefab == null)
        {
            Debug.LogError("SpawnManager: Cannot initialize. Missing required references.");
            return;
        }

        foreach (var character in characters)
        {
            GameObject buttonObj = Instantiate(spawnButtonPrefab, spawnButtonContainer);
            SpawnButton spawnButton = buttonObj.GetComponent<SpawnButton>();

            if (spawnButton == null)
            {
                Debug.LogError($"SpawnManager: The prefab {spawnButtonPrefab.name} is missing a SpawnButton component!");
                continue;
            }

            spawnButton.Initialize(character);
            spawnButtons.Add(spawnButton);
        }
    }

    public void SpawnCharacter(CharacterData character)
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("SpawnManager: No spawn points available!");
            return;
        }

        if (GameManager.Instance.TrySpendResources(character.cost))
        {
            // Get the next spawn point
            Transform spawnPoint = spawnPoints[currentSpawnPointIndex];
            
            // Spawn the character at the spawn point position
            Vector3 spawnPos = spawnPoint.position + character.spawnOffset;
            Instantiate(character.prefab, spawnPos, spawnPoint.rotation);
            
            // Move to the next spawn point (loop back to 0 if we reach the end)
            currentSpawnPointIndex = (currentSpawnPointIndex + 1) % spawnPoints.Length;
            
            Debug.Log($"{character.name} spawned at {spawnPos}");
        }
        else
        {
            Debug.LogWarning($"Failed to spawn {character.name}: Not enough resources.");
        }
    }
}