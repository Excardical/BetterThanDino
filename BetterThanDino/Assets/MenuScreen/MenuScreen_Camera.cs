using UnityEngine;
using UnityEngine.Tilemaps;

public class MenuScreen_Camera : MonoBehaviour
{
    public Transform player; 
    public Tilemap theMap;  

    private Vector3 bottomLeftLimit;  // Bottom-left corner of the camera boundary
    private Vector3 topRightLimit;    // Top-right corner of the camera boundary
    private float halfHeight;         // Half of the camera's height
    private float halfWidth;

    private void Start()
    {
        // Calculate camera dimensions based on orthographic size
        Camera cam = Camera.main;
        halfHeight = cam.orthographicSize;
        halfWidth = cam.aspect * halfHeight;

        // Calculate bottom-left and top-right limits based on the Tilemap's bounds
        Bounds tilemapBounds = theMap.localBounds;
        bottomLeftLimit = tilemapBounds.min + new Vector3(halfWidth, halfHeight, 0f);
        topRightLimit = tilemapBounds.max - new Vector3(halfWidth, halfHeight, 0f);
        topRightLimit.x -= 3f;
    }

    private void LateUpdate()
    {
        // Follow the player and clamp the camera's position within bounds
        Vector3 targetPosition = player.position;

        float clampedX = Mathf.Clamp(targetPosition.x, bottomLeftLimit.x, topRightLimit.x);
        float clampedY = Mathf.Clamp(targetPosition.y, bottomLeftLimit.y, topRightLimit.y);

        // Update the camera position
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}