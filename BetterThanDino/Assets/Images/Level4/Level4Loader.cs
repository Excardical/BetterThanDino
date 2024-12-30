using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level4Loader : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("Level4", LoadSceneMode.Single);
    }
}

