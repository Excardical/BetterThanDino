using UnityEngine;

public class DoorLabelTrigger : MonoBehaviour
{
    public GameObject player;       // The player GameObject
    public GameObject doorLabel;   // The text label GameObject
    public float triggerDistance = 2f; // Distance to show the label

    void Start()
    {
        // Hide the label initially
        doorLabel.SetActive(false);
    }

    void Update()
    {
        // Check the distance between the player and the door
        if (Vector2.Distance(player.transform.position, transform.position) <= triggerDistance)
        {
            doorLabel.SetActive(true);  // Show the label
        }
        else
        {
            doorLabel.SetActive(false); // Hide the label
        }
    }
}