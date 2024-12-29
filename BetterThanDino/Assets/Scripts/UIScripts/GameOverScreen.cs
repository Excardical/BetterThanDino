using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup; // CanvasGroup for fade effect.

    private void Start()
    {
        // Ensure the GameOver screen starts as invisible.
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ShowGameOverScreen()
    {
        StartCoroutine(FadeIn());
    }

    public void RestartButton()
    {
        Time.timeScale = 1; // Resume the game
        SceneManager.LoadScene("Level1");
    }
    
    public void RestartButtonLvl2()
    {
        Time.timeScale = 1; // Resume the game
        SceneManager.LoadScene("Level2");
    }

    public void ExitButton()
    {
        Time.timeScale = 1; // Resume the game
        SceneManager.LoadScene("MenuScreen");
    }

    private IEnumerator FadeIn()
    {
        float duration = 1f; // Duration of the fade.
        float elapsed = 0f;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // Use unscaled time
            canvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 1; // Ensure it's fully visible at the end.
    }
}
