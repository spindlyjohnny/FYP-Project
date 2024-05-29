using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    public static bool Return = false;
    public GameObject levelSelect;
    public virtual void LoadScene(int scene) { // load scene when UI button pressed.
        SceneManager.LoadScene(scene);
    }
    public void QuitGame() {
        Application.Quit();
    }
    private void Start()
    {
        if (Return == false) return;
        Return = true;
        levelSelect.SetActive(true);
    }
}
