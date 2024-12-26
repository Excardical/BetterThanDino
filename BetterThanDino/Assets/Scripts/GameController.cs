using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameOverScreen GameOverScreen;
    public BaseHealth baseHealth;
    private bool isGameOver = false;

    private void Update()
    {
        // Check if the player's health is 0 or below and the game is not already over.
        if (!isGameOver && StatsManager.Instance != null && StatsManager.Instance.currentHealth <= 0)
        {
            GameOver();
        }

        if (!isGameOver && baseHealth != null && baseHealth.currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        isGameOver = true;
        GameOverScreen.ShowGameOverScreen();
        StartCoroutine(PauseGameAfterFade());
    }

    private IEnumerator PauseGameAfterFade()
    {
        yield return new WaitForSecondsRealtime(1f); // Wait for fade-in to complete.
        // Time.timeScale = 0; // Pause the game.
    }
}