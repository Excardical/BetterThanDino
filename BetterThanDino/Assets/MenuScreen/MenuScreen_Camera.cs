using UnityEngine;
using UnityEngine.Tilemaps;

public class MenuScreen_Camera : MonoBehaviour
{
    public Transform player;          // Reference to the player
    public Tilemap theMap;            // Reference to the Tilemap

    private Vector3 bottomLeftLimit;  // Bottom-left corner of the camera boundary
    private Vector3 topRightLimit;    // Top-right corner of the camera boundary
    private float halfHeight;         // Half of the camera's height
    private float halfWidth;          // Half of the camera's width

    private void Start()
    {
        // Get the main camera
        Camera cam = Camera.main;

        // Calculate camera dimensions
        halfHeight = cam.orthographicSize;
        halfWidth = cam.aspect * halfHeight;

        // Get the bounds of the Tilemap
        Bounds tilemapBounds = theMap.localBounds;

        // Adjust the bottom-left and top-right limits based on camera dimensions
        bottomLeftLimit = tilemapBounds.min + new Vector3(halfWidth, halfHeight, 0f);
        topRightLimit = tilemapBounds.max - new Vector3(halfWidth, halfHeight, 0f);
    }

    private void LateUpdate()
    {
        // Get the player's current position
        Vector3 targetPosition = player.position;

        // Clamp the camera's position within the calculated bounds
        float clampedX = Mathf.Clamp(targetPosition.x, bottomLeftLimit.x, topRightLimit.x);
        float clampedY = Mathf.Clamp(targetPosition.y, bottomLeftLimit.y, topRightLimit.y);

        // Update the camera's position while keeping the Z-axis unchanged
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}