using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour {
    public Player player;
    public LevelManager levelManager;
    public GameObject pausescreen;
    NPCManagement npcmanager;
    // Start is called before the first frame update
    void Start() {
        player = FindObjectOfType<Player>();
        levelManager = FindObjectOfType<LevelManager>();
        npcmanager = FindObjectOfType<NPCManagement>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            if (pausescreen.activeSelf) ResumeGame();
            else PauseGame();
        }
    }
    public void PauseGame() {
        if (Time.timeScale == 0 || levelManager.gameover)
        {
            return;
        }
        AudioManager.instance.PauseMusic();
        Time.timeScale = 0; // freeze game
        pausescreen.SetActive(true);
        player.canMove = false;
    }
    public void ResumeGame() {
        Time.timeScale = 1;
        pausescreen.SetActive(false);
        if (npcmanager.myGeneral || npcmanager.myNPC || player.interactable) {
            AudioManager.instance.ResumeMusic();
            return;
        } 
        else {
            player.canMove = true;
            AudioManager.instance.ResumeMusic();
        }
    }
    public void BackToMainMenu() {
        SceneLoader.Return = true;
        Time.timeScale = 1;
        Player.list.Clear();
        SceneManager.LoadScene("MainMenu");
        AudioManager.instance.SwitchMusic(AudioManager.instance.titlemusic);
    }
    public void RestartLevel() {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
        levelManager.Initalize();
        //AudioManager.instance.PlayMusic(levelManager.levelmusic);
        //PlayerPrefs.SetInt("Current Room", 0);
    }
}