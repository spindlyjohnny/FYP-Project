using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {
    public static bool Return = false;
    public GameObject levelSelect,fpsCounter,tutorialPanel;
    [SerializeField] Image img;
    [SerializeField] Sprite[] checkbox;
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

    public void StopTutorial()
    {
        PlayerPrefs.SetInt("Tutorial", 1);
        print(PlayerPrefs.GetInt("Tutorial"));
        PlayerPrefs.Save();
    }
    private void Start()
    {
        tutorialPanel.SetActive(true);
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            PlayerPrefs.SetInt("Tutorial", 0);//0 is  false and 1 is true
            
        }
        if (PlayerPrefs.GetInt("Tutorial") == 1)
        {
            //tutorialPanel.SetActive(false);
        }
        print(PlayerPrefs.GetInt("Tutorial"));
        PlayerPrefs.SetInt("bool", 0);
        if (Return == false) return;
        Return = true;
        levelSelect.SetActive(true);
    }
    private void Update() {
        FPSCounter(FPS.GetCurrentFPS().ToString());
    }
    protected void FPSCounter(string fps) {
        if (PlayerPrefs.GetInt("FPS") == 1) {
            fpsCounter.GetComponent<TMP_Text>().text = "FPS:" + fps;
        } 
        else {
            fpsCounter.GetComponent<TMP_Text>().text = "";
        }
    }

    public void ShowFPS() {
        if (img.sprite == checkbox[0]) {
            img.sprite = checkbox[1];
            PlayerPrefs.SetInt("FPS", 1);
        } 
        else {
            img.sprite = checkbox[0];
            PlayerPrefs.SetInt("FPS", 0);
        }
    }
}
