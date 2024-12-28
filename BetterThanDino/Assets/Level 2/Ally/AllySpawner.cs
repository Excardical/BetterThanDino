using UnityEngine;
using UnityEngine.UI;

public class AllySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject allyPrefab;
    private GameObject storedAllyPrefab;  // Backup reference
    public Transform spawnPoint;
    public float spawnCooldown = 10f;
    private float currentCooldown;
    private bool canSpawn = true;

    [Header("UI Elements")]
    public Button spawnButton;
    public Image cooldownOverlay;

    private void Start()
    {
        // Store a backup of the prefab reference
        if (allyPrefab != null)
        {
            storedAllyPrefab = allyPrefab;
        }
        
        if (spawnButton != null)
        {
            spawnButton.onClick.AddListener(SpawnAlly);
        }
        
        currentCooldown = 0;
        UpdateUI();
    }

    private void Update()
    {
        if (!canSpawn)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0)
            {
                canSpawn = true;
                currentCooldown = 0;
            }
            UpdateUI();
        }
    }

    public void SpawnAlly()
    {
        if (!canSpawn) return;

        // Use stored prefab if main reference is lost
        GameObject prefabToUse = allyPrefab != null ? allyPrefab : storedAllyPrefab;

        if (prefabToUse != null && spawnPoint != null)
        {
            GameObject newAlly = Instantiate(prefabToUse, spawnPoint.position, Quaternion.identity);
        }

        canSpawn = false;
        currentCooldown = spawnCooldown;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (spawnButton != null)
        {
            spawnButton.interactable = canSpawn;
        }

        if (cooldownOverlay != null)
        {
            cooldownOverlay.fillAmount = currentCooldown / spawnCooldown;
            cooldownOverlay.gameObject.SetActive(!canSpawn);
        }
    }
}