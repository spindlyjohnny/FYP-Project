using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour {
    public static bool Return = false;
    public GameObject levelSelect,fpsCounter,tutorialPanel;
    public virtual void LoadScene(int scene,bool async = false) { // load scene from levelmanager
        if (async) SceneManager.LoadSceneAsync(scene);
        else SceneManager.LoadScene(scene);
    }
    public virtual void LoadScene(int scene) { // load scene when UI button pressed.
        SceneManager.LoadScene(scene);
        if(scene == 1) { // bus scene
            if (PlayerPrefs.GetInt("bool") == 0) {
                PlayerPrefs.SetFloat("energy", 100);
                PlayerPrefs.SetFloat("Max Energy", 100);
                PlayerPrefs.SetFloat("Invincibility Time", 5f);
                //maxenergy = PlayerPrefs.GetFloat("Max Energy");
                ////energygain = PlayerPrefs.GetFloat("Energy Gain");
                //originalInvincibleTime = PlayerPrefs.GetFloat("Invincibility Time");
            }
        }
    }
    public void SetLevel(int levelNum) {
        LevelManager.levelNum = (LevelManager.LevelNum)levelNum;
        PlayerPrefs.SetInt("Level Num", levelNum);
    }
    private void Start()
    {
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            PlayerPrefs.SetInt("Tutorial", 0);//0 is  false and 1 is true            
        }
        if (PlayerPrefs.GetInt("Tutorial") == 1)
        {
            tutorialPanel.SetActive(false);
        }
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
