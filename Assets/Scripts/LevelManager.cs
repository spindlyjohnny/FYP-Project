using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelManager : SceneLoader {
    public bool gameover;
    public GameObject gameoverscreen, taskcompletescreen, loadingscreen;
    public Slider energyslider;
    public GameObject[] tiles;
    [SerializeField] GameObject[] bustiles;
    public GameObject mrt;
    public int score, tileindex;
    public TMP_Text scoretext, tasksuccesstext;
    NPCManagement npcmanager;
    //GameObject currenttile;
    int tileshiftfactor;
    public TMP_Text dialoguetext, questiontext, explaintext, optionAtext, optionBtext, optionCtext, optionDtext;
    public TMP_Text nametext;
    public GameObject optionAButton, optionBButton, optionCButton, optionDButton;
    public GameObject dialoguebox, questionbox, busstart;
    int numberOfTiles = 5;
    public float tilerng;
    public enum Level { Bus, MRT };
    public static Level level;
    public Tile[] currenttiles;
    CameraController cam;
    public Sprite[] loadingimgs;
    Player player;
    public GameObject upgradeText,boost;
    //[SerializeField]Tile starttile;
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(AudioManager.instance.SwitchMusic(AudioManager.instance.levelmusic));
        //score = 0;
        //tileshiftfactor = 0;
        npcmanager = FindObjectOfType<NPCManagement>();
        gameover = false;
        gameoverscreen.SetActive(false);
        cam = FindObjectOfType<CameraController>();
        player = FindObjectOfType<Player>();
        //tileindex = UnityEngine.Random.Range(0, tiles.Length);
        if (level == Level.Bus) {
            cam.transform.position = cam.originalposition.position;
            cam.lookOffset = cam.defaultoffset;
            for (int i = 0; i < tiles.Length; i++) {
                tiles[i] = bustiles[i];
            }
            Spawn(8);
            RandomTile();
        } else {
            cam.transform.position = cam.trainposition.position;
            cam.lookOffset = cam.trainoffset;
            for (int i = 0; i < tiles.Length; i++) {
                tiles[i] = mrt;
            }
            tileindex = 0;
            Spawn(8);
            player.transform.position = FindObjectOfType<Tile>().transform.Find("Player Start Point").position;
        }
    }

    // Update is called once per frame
    void Update() {
        print("Level:" + level);
        //tilerng = UnityEngine.Random.Range(0f, 1f);
        if (level == Level.Bus) RandomTile();
        //else {
        //    for (int i = 0; i < tiles.Length; i++) {
        //        tiles[i] = mrt;
        //    }
        //    tileindex = 0;
        //}
        print("Shift:" + tileshiftfactor);
        if (gameover) gameoverscreen.SetActive(true);
        scoretext.text = "Score:" + score;
        if (npcmanager.myNPC != null) {
            if (npcmanager.myNPC.tasksuccess == NPC.Task.Fail) {
                tasksuccesstext.text = "Task failed!";
            } else if (npcmanager.myNPC.tasksuccess == NPC.Task.Success) {
                tasksuccesstext.text = "Task success!";
            }
        }
        if (taskcompletescreen.activeSelf) StartCoroutine(DisableTaskScreen());
        if (loadingscreen.activeSelf) StartCoroutine(DisableLoadingScreen(2f));
        currenttiles = FindObjectsOfType<Tile>();
        if (currenttiles.Length == 1 || level == Level.MRT) {
            tileshiftfactor = 0;
        } 
        else if (level == Level.Bus) {
            tileshiftfactor = 21;
        } 
            //else {
            //    tileshiftfactor = 0;
            //    //tileshiftfactor = Mathf.RoundToInt(26.5f * 3);
            //}
        if (currenttiles.Length > 10) {
            if (currenttiles[currenttiles.Length - 1].gameObject != busstart) {
                Destroy(currenttiles[currenttiles.Length - 1].gameObject);
            } 
            else {
                currenttiles[currenttiles.Length - 1].gameObject.SetActive(false);
            }
            player.inputtext.SetActive(false);
        }
        //tileshiftfactor = currenttiles.Length == 1 ? 0 : 21; // tileshiftfactor spawns tiles 21 units ahead because when player enters trigger, there are 3 tiles in front. each tile is 7 units long on the x-axis
    }
    public void RestartLevel() {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    IEnumerator DisableTaskScreen() {
        yield return new WaitForSeconds(2f);
        taskcompletescreen.SetActive(false);
    }
    IEnumerator DisableLoadingScreen(float time) {
        yield return new WaitForSeconds(time);
        loadingscreen.SetActive(false);
    }
    public void MoveToTrain() {

        print("yes2");
        LoadScene(2);
        loadingscreen.SetActive(true);
        loadingscreen.GetComponent<Image>().sprite = loadingimgs[UnityEngine.Random.Range(0, loadingimgs.Length)];
        //numberOfTiles = 5;
        //Spawn(8);
        //StartCoroutine(DisableLoadingScreen(2f));
        //foreach (var i in FindObjectsOfType<Tile>()) {
        //    if (!i.gameObject.CompareTag("Train") && i.gameObject != busstart) {
        //        Destroy(i.gameObject);
        //    } else if (i.gameObject.CompareTag("Train") && i.GetComponent<TrainTile>()) {
        //        player.transform.position = i.transform.Find("Player Start Point").position;
        //    }
        //}
        //cam.transform.position = cam.trainposition.position;
        //cam.lookOffset = cam.trainoffset;
    }
    public void MoveToBus() {

        print("yuh");
        //busstart.SetActive(true);
        //starttile = busstart.GetComponent<Tile>();
        loadingscreen.SetActive(true);
        loadingscreen.GetComponent<Image>().sprite = loadingimgs[UnityEngine.Random.Range(0, loadingimgs.Length)];
        LoadScene(1);
        //for (int i = 0; i < tiles.Length; i++) {
        //    tiles[i] = bustiles[i];
        //}
        //numberOfTiles = 5;
        //tileshiftfactor = 0;
        //Spawn(8);
        //StartCoroutine(DisableLoadingScreen(2f));
        //foreach (var i in FindObjectsOfType<Tile>()) {
            
        //    if (i.gameObject.CompareTag("Train") && !i.GetComponent<RoadTile>() && i.gameObject != busstart) {
        //        Destroy(i.gameObject);
        //    } else if (i.gameObject == busstart) {
        //        player.transform.position = i.transform.Find("Player Start Point").position;
        //    }
        //}
        //cam.transform.position = cam.originalposition.position;
        //cam.lookOffset = cam.defaultoffset;
        //level = Level.Bus;
    }
    public void Spawn(int amount) {
        //if(level == Level.Bus)tilerng = UnityEngine.Random.Range(0f, 1f);
        for (int x = 0; x < amount; x++) { // spawn amount tiles at a time
            Tile mytile;
            if (amount == 1) {
                x = numberOfTiles;
            }
            mytile = tiles[tileindex].GetComponent<Tile>();
            //if (busstart.activeSelf) {
            //    mytile = busstart.GetComponent<Tile>();
            //} 
            //else {
            //    mytile = tiles[tileindex].GetComponent<Tile>();
            //}
            // spawn tile at spawn point + size of tile * order that tile was spawned
            //print(mytile.spawnpt.position);
            if (level == Level.Bus) {
                Instantiate(tiles[tileindex], mytile.spawnpt.position + new Vector3(7 * x, 0, 0) + new Vector3(tileshiftfactor, 0, 0), Quaternion.identity);
            } else {
                Instantiate(tiles[tileindex], mytile.spawnpt.position + new Vector3(26.5f * x, 0, 0) + new Vector3(tileshiftfactor, 0, 0), Quaternion.identity);
            }
            //print("Yes");
            if (amount == 1) numberOfTiles += 1;
        }
    }
    void RandomTile() {
        tilerng = UnityEngine.Random.Range(0f, 1f);
        if (tilerng <= .5f && tilerng > 0) {
            foreach (var i in tiles) {
                if (i.GetComponent<RoadTile>()) tileindex = Array.IndexOf(tiles, i);
            }
        } else {
            List<int> legalindexes = new List<int>();
            for (int i = 0; i < tiles.Length; i++) {
                if (!tiles[i].GetComponent<RoadTile>()) legalindexes.Add(i);
            }
            System.Random random = new System.Random();
            tileindex = legalindexes[random.Next(0, legalindexes.Count)];
        }
    }

}
