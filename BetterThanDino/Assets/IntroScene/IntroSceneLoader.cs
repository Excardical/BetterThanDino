using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneLoader : MonoBehaviour
{
    private void OnEnable()
    {
        //Only specify the sceneName or sceneBuildIntex will load the scene with the single mode
        SceneManager.LoadScene("MenuScreen", LoadSceneMode.Single);
    }
}