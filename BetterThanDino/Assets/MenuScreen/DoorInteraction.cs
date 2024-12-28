using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteraction : MonoBehaviour
{
    public GameObject player;          // Reference to the player GameObject
    public string sceneName;           // Name of the scene to load
    public GameObject popupText;       // Reference to the pop-up text
    private bool isPlayerNear = false; // Tracks if the player is near the door

    public Vector3 offset = new Vector3(0, 1.5f, 0); // Offset for the pop-up text
    private RectTransform popupTransform; // RectTransform of the pop-up text
    private Camera mainCamera;           // Reference to the main camera

    void Start()
    {
        // Get references
        popupTransform = popupText.GetComponent<RectTransform>();
        mainCamera = Camera.main;

        // Hide the pop-up text at the start
        popupText.SetActive(false);
    }

    void Update()
    {
        // Check if the player is near and presses the "E" key
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            // Load the specified scene
            SceneManager.LoadScene(sceneName);
        }

        // If the pop-up text is active, update its position to stay above the player
        if (popupText.activeSelf)
        {
            UpdatePopupPosition();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Detect if the player enters the trigger
        if (other.gameObject == player)
        {
            isPlayerNear = true; // Player is now near the door
            popupText.SetActive(true); // Show the pop-up text
            UpdatePopupPosition(); // Update its initial position
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Detect if the player leaves the trigger
        if (other.gameObject == player)
        {
            isPlayerNear = false; // Player is no longer near the door
            popupText.SetActive(false); // Hide the pop-up text
        }
    }

    void UpdatePopupPosition()
    {
        // Convert player's world position to screen position with an offset
        Vector3 worldPositionWithOffset = player.transform.position + offset;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPositionWithOffset);

        // Apply the new position to the pop-up text
        popupTransform.position = screenPosition;
    }
}
