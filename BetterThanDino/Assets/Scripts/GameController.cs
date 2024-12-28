using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameOverScreen GameOverScreen;
    public WinScreen WinScreen;
    public BaseHealth baseHealth;
    private bool isGameOver = false;
    private bool isWin = false;

    private void Update()
    {
        // Check for lose conditions
        if (!isWin && !isGameOver && StatsManager.Instance != null && StatsManager.Instance.currentHealth <= 0)
        {
            GameOver();
        }

        if (!isWin && !isGameOver && baseHealth != null && baseHealth.currentHealth <= 0)
        {
            GameOver();
        }

        // Check win condition continuously
        if (!isWin && !isGameOver)
        {
            CheckWinCondition();
        }
    }

    public void CheckWinCondition()
    {
        if (WaveManager.Instance != null)
        {
            // Win if wave is complete AND all enemies are defeated
            if (WaveManager.Instance.IsWaveComplete && WaveManager.Instance.AreAllEnemiesDefeated)
            {
                Win();
            }
        }
    }

    public void Win()
    {
        Debug.Log("Victory!");
        isWin = true;
        WinScreen.ShowWinScreen();
        StartCoroutine(PauseGameAfterFade());
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