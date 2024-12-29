using UnityEngine;
using UnityEngine.UI;

public class AllySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject allyKnightPrefab;
    public GameObject allyArcherPrefab;
    public Transform spawnPoint;

    public float knightCooldown = 10f;
    public float archerCooldown = 5f;

    private float knightCurrentCooldown = 0f;
    private float archerCurrentCooldown = 0f;

    private bool canSpawnKnight = true;
    private bool canSpawnArcher = true;

    [Header("UI Elements")]
    public Button spawnKnightButton;
    public Button spawnArcherButton;
    public Image knightCooldownOverlay;
    public Image archerCooldownOverlay;

    private void Start()
    {
        if (spawnKnightButton != null)
        {
            spawnKnightButton.onClick.AddListener(SpawnKnight);
        }

        if (spawnArcherButton != null)
        {
            spawnArcherButton.onClick.AddListener(SpawnArcher);
        }

        UpdateUI();
    }

    private void Update()
    {
        // Handle cooldown for Knight
        if (!canSpawnKnight)
        {
            knightCurrentCooldown -= Time.deltaTime;
            if (knightCurrentCooldown <= 0)
            {
                canSpawnKnight = true;
                knightCurrentCooldown = 0;
            }
        }

        // Handle cooldown for Archer
        if (!canSpawnArcher)
        {
            archerCurrentCooldown -= Time.deltaTime;
            if (archerCurrentCooldown <= 0)
            {
                canSpawnArcher = true;
                archerCurrentCooldown = 0;
            }
        }

        UpdateUI();
    }

    public void SpawnKnight()
    {
        if (!canSpawnKnight || allyKnightPrefab == null || spawnPoint == null) return;

        Instantiate(allyKnightPrefab, spawnPoint.position, Quaternion.identity);

        canSpawnKnight = false;
        knightCurrentCooldown = knightCooldown;

        UpdateUI();
    }

    public void SpawnArcher()
    {
        if (!canSpawnArcher || allyArcherPrefab == null || spawnPoint == null) return;

        Instantiate(allyArcherPrefab, spawnPoint.position, Quaternion.identity);

        canSpawnArcher = false;
        archerCurrentCooldown = archerCooldown;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (spawnKnightButton != null)
        {
            spawnKnightButton.interactable = canSpawnKnight;
        }

        if (knightCooldownOverlay != null)
        {
            knightCooldownOverlay.fillAmount = knightCurrentCooldown / knightCooldown;
            knightCooldownOverlay.gameObject.SetActive(!canSpawnKnight);
        }

        if (spawnArcherButton != null)
        {
            spawnArcherButton.interactable = canSpawnArcher;
        }

        if (archerCooldownOverlay != null)
        {
            archerCooldownOverlay.fillAmount = archerCurrentCooldown / archerCooldown;
            archerCooldownOverlay.gameObject.SetActive(!canSpawnArcher);
        }
    }
}
