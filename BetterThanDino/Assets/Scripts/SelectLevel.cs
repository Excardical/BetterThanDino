using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour {
    public void GoLevel1() {
        SceneManager.LoadSceneAsync(2);
    }

    public void GoLevel2() {
        SceneManager.LoadSceneAsync(3);
    }
    public void GoLevel3() {
        SceneManager.LoadSceneAsync(4);
    }
    public void GoLevel4() {
        SceneManager.LoadSceneAsync(5);
    }
    public void GoLevel5() {
        SceneManager.LoadSceneAsync(6);
    }

    public void GoPrev() {
        SceneManager.LoadSceneAsync(0);
    }
}