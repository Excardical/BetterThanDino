using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup; // CanvasGroup for fade effect.

    private void Start()
    {
        // Ensure the GameOver screen starts as invisible.
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ShowWinScreen()
    {
        StartCoroutine(FadeIn());
    }

    public void NextLevel()
    {
        Time.timeScale = 1; // Resume the game

        // Get the current scene index and load the next scene.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index is within bounds of the build settings.
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more levels to load!");
            // Optionally, load a win screen or main menu.
            SceneManager.LoadScene("MenuScreen");
        }
    }

    public void RestartButton()
    {
        Time.timeScale = 1; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart current scene
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
