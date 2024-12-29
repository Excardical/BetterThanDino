using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Resources")]
    [SerializeField] private float startingResources = 1000f;
    [SerializeField] private float resourceGainRate = 10f;
    [SerializeField] private TextMeshProUGUI resourceText;
    [SerializeField] private GameObject spawnUI;
    [SerializeField] private GameObject resourceDisplay;

    private float currentResources;
    private List<CharacterData> selectedRoster;
    private bool gameStarted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentResources = startingResources;
        UpdateResourceUI();

        // Ensure UI is hidden at the start
        spawnUI.SetActive(false);
        resourceDisplay.SetActive(false);

        Time.timeScale = 0;
    }

    private void Update()
    {
        if (gameStarted)
        {
            currentResources += resourceGainRate * Time.deltaTime;
            UpdateResourceUI();
        }
    }

    private void UpdateResourceUI()
    {
        resourceText.text = $"Resources: {Mathf.Floor(currentResources)}";
    }

    public void StartGameWithCharacters(List<CharacterData> characters)
    {
        selectedRoster = characters;

        // Show the resource display and spawn UI
        resourceDisplay.SetActive(true);
        spawnUI.SetActive(true);

        // Initialize spawning system
        SpawnManager.Instance.Initialize(selectedRoster);

        // Mark game as started
        gameStarted = true;
    }

    public bool TrySpendResources(float amount)
    {
        Debug.Log($"Attempting to spend {amount} resources. Current: {currentResources}");
        if (currentResources >= amount)
        {
            currentResources -= amount;
            UpdateResourceUI();
            Debug.Log($"Resources spent. Remaining: {currentResources}");
            return true;
        }
        Debug.LogWarning("Insufficient resources.");
        return false;
    }
}
