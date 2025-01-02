using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level5Loader : MonoBehaviour
{
    private void OnEnable()
    {
        //Only specify the sceneName or sceneBuildIntex will load the scene with the single mode
        SceneManager.LoadScene("Level5", LoadSceneMode.Single);
    }
}
