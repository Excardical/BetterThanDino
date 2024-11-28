using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void GoPlay() {
        SceneManager.LoadSceneAsync(1);
    }
    public void GoQuit() {
        Application.Quit();
    }
}
