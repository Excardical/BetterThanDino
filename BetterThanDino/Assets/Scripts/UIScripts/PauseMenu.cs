using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup pauseMenuCanvasGroup;
    [SerializeField] private GameObject pauseButton;
    private bool pauseOpen = false;

    void Update()
    {
        if (Input.GetButtonDown("TogglePause"))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        pauseOpen = !pauseOpen;
        Debug.Log($"Pause toggled. IsPaused: {pauseOpen}");

        if (pauseOpen)
        {
            ShowPauseMenu();
            pauseButton.SetActive(false);
            Time.timeScale = 0;
        }
        else
        {
            HidePauseMenu();
            pauseButton.SetActive(true);
            Time.timeScale = 1;
        }
    }

    private void ShowPauseMenu()
    {
        pauseMenuCanvasGroup.alpha = 1; // Make the canvas visible
        pauseMenuCanvasGroup.interactable = true; // Enable interaction
        pauseMenuCanvasGroup.blocksRaycasts = true; // Allow raycasts to pass through
    }

    private void HidePauseMenu()
    {
        pauseMenuCanvasGroup.alpha = 0; // Make the canvas invisible
        pauseMenuCanvasGroup.interactable = false; // Disable interaction
        pauseMenuCanvasGroup.blocksRaycasts = false; // Block raycasts
    }

    public void Pause()
    {
        TogglePause();
    }

    public void Home()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        TogglePause();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
