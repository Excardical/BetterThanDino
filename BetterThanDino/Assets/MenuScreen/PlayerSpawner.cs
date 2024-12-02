using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; 
    public Transform spawnPoint;   
    private void Start()
    {
        // Spawn the player at the predetermined spawn point
        Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
    }
}