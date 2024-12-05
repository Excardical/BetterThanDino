using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject pauseButton;
    private bool pauseOpen = false;

    // private void Update()
    // {
    //     if (Input.GetButtonDown("TogglePause"))
    //     {
    //         Debug.Log("TogglePause button pressed");
    //         if (!pauseOpen)
    //         {
    //             pauseButton.SetActive(false);
    //             pauseMenu.SetActive(true);
    //             Time.timeScale = 0;
    //             pauseOpen = true;
    //         }
    //         else
    //         {
    //             pauseButton.SetActive(true);
    
    //             pauseMenu.SetActive(false);
    //             Time.timeScale = 1;
    //             pauseOpen = false;
    //         }
    //     }
    // }
    public void Pause()
    {
        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        pauseOpen = true;
    }


    public void Home()
    {
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        SceneManager.LoadSceneAsync(0);
        Time.timeScale = 1;
        pauseOpen = false;
    }
    public void Resume()
    {
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        pauseOpen = false;
    }
    public void Restart()
    {
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        pauseOpen = false;
    }
}
