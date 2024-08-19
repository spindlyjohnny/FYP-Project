using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {
    public static bool Return = false;
    public GameObject levelSelect,fpsCounter,tutorialPanel;
    [SerializeField] Image tutimg;
    [SerializeField] Sprite[] checkbox;
    [SerializeField] GameObject tutpanel;
    public Button L2button,L3button;
    bool fps;
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
            }
        }
    }
    public void SetLevel(int levelNum) {
        LevelManager.levelNum = (LevelManager.LevelNum)levelNum;
        PlayerPrefs.SetInt("Level Num", levelNum);
    }
    public void SetLevelStatus(string level) {
        if (!PlayerPrefs.HasKey(level)) {
            PlayerPrefs.SetInt(level, 1);
        }
    }
    public void OpenLevelSelect() {
        levelSelect.SetActive(true);
        if(PlayerPrefs.GetInt("Show Tutorial", 0) == 0) {
            tutpanel.SetActive(true);
        } 
        else {
            tutpanel.SetActive(false);
        }
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
        //img.sprite = checkbox[PlayerPrefs.GetInt("FPS")];
        tutimg.sprite = checkbox[PlayerPrefs.GetInt("Show Tutorial",0)];
        tutpanel.SetActive(PlayerPrefs.GetInt("Show Tutorial", 0) != 1);
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            PlayerPrefs.SetInt("Tutorial", 0);//0 is  false and 1 is true
            
        }
        if (PlayerPrefs.GetInt("Tutorial") == 1)
        {
            //tutorialPanel.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Level 2") == 1) {
            L2button.interactable = true;
        } 
        else {
            L2button.interactable = false;
        }
        if (PlayerPrefs.GetInt("Level 3") == 1) {
            L3button.interactable = true;
        } 
        else {
            L3button.interactable = false;
        }
        print(PlayerPrefs.GetInt("Tutorial"));
        PlayerPrefs.SetInt("bool", 0);
        if (Return == false) return;
        Return = true;
        levelSelect.SetActive(true);
    }
    private void Update() {
        FPSCounter(FPS.GetCurrentFPS().ToString());
        if (Input.GetKeyDown(KeyCode.Tab)) ShowFPS();
    }
    protected void FPSCounter(string fps) {
        if (PlayerPrefs.GetInt("FPS") == 1) {
            fpsCounter.gameObject.SetActive(true);
            fpsCounter.GetComponent<TMP_Text>().text = "FPS:" + fps;
        } 
        else {
            fpsCounter.GetComponent<TMP_Text>().text = "";
            fpsCounter.gameObject.SetActive(false);
        }
    }

    public void ShowFPS() {
        fps = !fps;
        PlayerPrefs.SetInt("FPS", fps ? 1 : 0);
    }
    public void HideTutorial() {
        if (tutimg.sprite == checkbox[0]) {
            tutimg.sprite = checkbox[1];
            tutpanel.SetActive(false);
            PlayerPrefs.SetInt("Show Tutorial", 1);
        } 
        else {
            tutimg.sprite = checkbox[0];
            tutpanel.SetActive(true);
            PlayerPrefs.SetInt("Show Tutorial", 0);
        }
    }
}
