using UnityEngine;
using UnityEngine.UI; // For UI elements
using TMPro; // For TextMeshPro UI (if you're using it)

public class AllySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject allyPrefab;
    public Transform spawnPoint;
    public float spawnCooldown = 10f;
    private float currentCooldown;
    private bool canSpawn = true;

    [Header("UI Elements")]
    public Button spawnButton;
    public Image cooldownFill; // Optional: for a visual cooldown indicator
    public TextMeshProUGUI cooldownText; // Optional: for numerical countdown

    private void Start()
    {
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

        // Spawn the ally at the spawn point
        GameObject newAlly = Instantiate(allyPrefab, spawnPoint.position, Quaternion.identity);
    
        // Get and reset the animator
        Animator animator = newAlly.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("Idle"); // Replace "Idle" with your default animation state name
            animator.ResetTrigger("Attack"); // Reset any attack triggers you have
        }

        // Start cooldown
        canSpawn = false;
        currentCooldown = spawnCooldown;
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Update button interactability
        if (spawnButton != null)
        {
            spawnButton.interactable = canSpawn;
        }

        // Update cooldown fill if you have one
        if (cooldownFill != null)
        {
            cooldownFill.fillAmount = currentCooldown / spawnCooldown;
        }

        // Update cooldown text if you have one
        if (cooldownText != null)
        {
            cooldownText.text = canSpawn ? "Ready!" : Mathf.Ceil(currentCooldown).ToString();
        }
    }
}