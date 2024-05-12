using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : SceneLoader {
    public bool gameover;
    public GameObject gameoverscreen,taskcompletescreen;
    public Slider energyslider;
    SpawnTiles tiles;
    public int score,credits;
    public TMP_Text scoretext,tasksuccesstext,creditstext;
    NPCManagement npcmanager;
    // Start is called before the first frame update
    void Start()
    {
        tiles = FindObjectOfType<SpawnTiles>();
        score = 0;
        credits = 0;
        npcmanager = FindObjectOfType<NPCManagement>();
        tiles.Spawn(8);
        gameover = false;
        gameoverscreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameover)gameoverscreen.SetActive(true);
        scoretext.text = "Score:" + score;
        creditstext.text = "Credits:" + credits;
        if(npcmanager.myNPC != null) {
            if (npcmanager.myNPC.tasksuccess == NPC.Task.Fail) {
                tasksuccesstext.text = "Task failed!";
            } 
            else if (npcmanager.myNPC.tasksuccess == NPC.Task.Success) {
                tasksuccesstext.text = "Task success!";
            }
        }
        if (taskcompletescreen.activeSelf) StartCoroutine(DisableTaskScreen());
    }
    public void RestartLevel() {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    IEnumerator DisableTaskScreen() {
        yield return new WaitForSeconds(2f);
        taskcompletescreen.SetActive(false);
    }
}
