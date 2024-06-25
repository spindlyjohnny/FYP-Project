using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour {
    public static bool Return = false;
    public GameObject levelSelect,fpsCounter;
    public virtual void LoadScene(int scene,bool async = false) { // load scene when UI button pressed.
        if (async) SceneManager.LoadSceneAsync(scene);
        else SceneManager.LoadScene(scene);
    }
    public void QuitGame() {
        Application.Quit();
    }
    private void Start()
    {
        PlayerPrefs.SetInt("bool", 0);
        if (Return == false) return;
        Return = true;
        levelSelect.SetActive(true);
    }
    private void Update() {
        ShowFPS(FPS.GetCurrentFPS().ToString());
    }
    protected void ShowFPS(string fps) {
        fpsCounter.GetComponent<TMP_Text>().text = "FPS:" + fps;
    }
}
