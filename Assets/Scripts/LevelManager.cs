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
    [SerializeField]GameObject[] bustiles;
    public GameObject mrt;
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
    public Tile[] currenttiles;
    CameraController cam;
    public Sprite[] loadingimgs;
    Player player;
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(AudioManager.instance.SwitchMusic(AudioManager.instance.levelmusic));
        score = 0;
        tileshiftfactor = 0;
        npcmanager = FindObjectOfType<NPCManagement>();
        for(int i = 0; i < tiles.Length; i++) {
            tiles[i] = bustiles[i];
        }
        //tileindex = UnityEngine.Random.Range(0, tiles.Length);
        Spawn(8);
        RandomTile();
        gameover = false;
        gameoverscreen.SetActive(false);
        cam = FindObjectOfType<CameraController>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update() {
        //tilerng = UnityEngine.Random.Range(0f, 1f);
        if(level == Level.Bus)RandomTile();
        else {
            for (int i = 0; i < tiles.Length; i++) {
                tiles[i] = mrt;
            }
            tileindex = 0;
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
        currenttiles = FindObjectsOfType<Tile>();
        if(currenttiles.Length == 1) {
            tileshiftfactor = 0;
        } 
        else {
            if(level == Level.Bus) {
                tileshiftfactor = 21;
            } 
            else {
                tileshiftfactor = 0;
                //tileshiftfactor = Mathf.RoundToInt(26.5f * 3);
            }
        }
        if (currenttiles.Length > 10)
        {
            Destroy(currenttiles[currenttiles.Length - 1].gameObject);
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
        loadingscreen.SetActive(true);
        loadingscreen.GetComponent<Image>().sprite = loadingimgs[UnityEngine.Random.Range(0, loadingimgs.Length)];
        numberOfTiles = 5;
        Spawn(8);
        StartCoroutine(DisableLoadingScreen(2f));
        foreach (var i in FindObjectsOfType<Tile>())
        {
            if (!i.gameObject.CompareTag("Train") && !i.gameObject.CompareTag("Bus"))
            {
                Destroy(i.gameObject);
            }
            else if(i.gameObject.CompareTag("Train") && i.GetComponent<TrainTile>()){
                player.transform.position = i.transform.Find("Player Start Point").position;
            }
        }
        cam.transform.position = cam.trainposition.position;
        cam.lookOffset = cam.trainoffset;
    }
    public void MoveToBus() {

        print("yuh");
        loadingscreen.SetActive(true);
        loadingscreen.GetComponent<Image>().sprite = loadingimgs[UnityEngine.Random.Range(0, loadingimgs.Length)];
        for (int i = 0; i < tiles.Length; i++) {
            tiles[i] = bustiles[i];
        }
        numberOfTiles = 5;
        Spawn(8);
        StartCoroutine(DisableLoadingScreen(2f));
        foreach (var i in FindObjectsOfType<Tile>()) {
            if (i.gameObject.CompareTag("Train") && !i.GetComponent<RoadTile>() && !i.CompareTag("Bus")) {
                Destroy(i.gameObject);
            } 
            else if (i.name == "Bus Start") {
                player.transform.position = i.transform.Find("Player Start Point").position;
            }
        }
        cam.transform.position = cam.originalposition.position;
        cam.lookOffset = cam.defaultoffset;
        level = Level.Bus;
    }
    public void Spawn(int amount) {
        //if(level == Level.Bus)tilerng = UnityEngine.Random.Range(0f, 1f);
        for (int x = 0; x < amount; x++) { // spawn amount tiles at a time
            if (amount == 1) x = numberOfTiles;
            // spawn tile at spawn point + size of tile * order that tile was spawned
            Tile mytile = tiles[tileindex].GetComponent<Tile>();
            //print(mytile.spawnpt.position);
            if(level == Level.Bus) {
                Instantiate(tiles[tileindex], mytile.spawnpt.position + new Vector3(7 * x, 0, 0) + new Vector3(tileshiftfactor, 0, 0), Quaternion.identity);
            } 
            else {
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

    public void Destruction(GameObject tile)
    {

    }
}
