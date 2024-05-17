using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelManager : SceneLoader {
    public bool gameover;
    public GameObject gameoverscreen, taskcompletescreen,loadingscreen;
    public Slider energyslider;
    public GameObject[] tiles;
    public int score, tileindex;
    public TMP_Text scoretext, tasksuccesstext;
    NPCManagement npcmanager;
    //GameObject currenttile;
    int tileshiftfactor;
    public TMP_Text dialoguetext, questiontext, explaintext, optionAtext, optionBtext, optionCtext, optionDtext;
    public TMP_Text nametext;
    public GameObject optionAButton, optionBButton, optionCButton, optionDButton;
    public GameObject dialoguebox, questionbox;
    int numberOfTiles=5;
    public float tilerng;
    public enum Level { Bus, MRT };
    public Level level;
    // Start is called before the first frame update
    void Start() {
        score = 0;
        tileshiftfactor = 0;
        npcmanager = FindObjectOfType<NPCManagement>();
        Spawn(8);
        gameover = false;
        gameoverscreen.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (npcmanager.myNPC) {
            foreach (var i in tiles) {
                if (i.GetComponent<RoadTile>()) tileindex = Array.IndexOf(tiles, i);
            }
        } 
        else {
            tileindex = UnityEngine.Random.Range(0, tiles.Length);
        }
        if (gameover) gameoverscreen.SetActive(true);
        scoretext.text = "Score:" + score;
        if (npcmanager.myNPC != null) {
            if (npcmanager.myNPC.tasksuccess == NPC.Task.Fail) {
                tasksuccesstext.text = "Task failed!";
            } 
            else if (npcmanager.myNPC.tasksuccess == NPC.Task.Success) {
                tasksuccesstext.text = "Task success!";
            }
        }
        if (taskcompletescreen.activeSelf) StartCoroutine(DisableTaskScreen());
        Tile[] currenttiles = FindObjectsOfType<Tile>();
        tileshiftfactor = currenttiles.Length == 1 ? 0 : 21; // tileshiftfactor spawns tiles 21 units ahead because when player enters trigger, there are 3 tiles in front. each tile is 7 units long on the x-axis
    }
    public void RestartLevel() {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    IEnumerator DisableTaskScreen() {
        yield return new WaitForSeconds(2f);
        taskcompletescreen.SetActive(false);
    }
    public void Spawn(int amount) {
        for (int x = 0; x < amount; x++) { // spawn amount tiles at a time
            if (amount == 1) x = numberOfTiles;
            // spawn tile at spawn point + size of tile * order that tile was spawned
            //if (!npcmanager.myNPC) {
            //    tileindex = UnityEngine.Random.Range(0, tiles.Length);
            //    print("Tile index:" + tileindex);
            //} else {
            //    foreach (var i in tiles) {
            //        if (i.GetComponent<RoadTile>()) tileindex = Array.IndexOf(tiles, i); print("Tile index:" + tileindex);
            //    }
            //}
            Tile mytile = tiles[tileindex].GetComponent<Tile>();
            print(mytile.spawnpt.position);
            Instantiate(tiles[tileindex], mytile.spawnpt.position + new Vector3(7 * x, 0, 0) + new Vector3(tileshiftfactor, 0, 0), Quaternion.identity);
            print("Yes");
            if (amount == 1) numberOfTiles += 1;
        }
    }
}
