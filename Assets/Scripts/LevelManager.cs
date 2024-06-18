using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
public class LevelManager : SceneLoader {
    public bool gameover;
    public GameObject gameoverscreen, taskcompletescreen, loadingscreen;
    public Slider energyslider;
    public GameObject[] tiles;
    [SerializeField] GameObject[] bustiles;
    public GameObject mrt;
    public int score; 
    public int tileindex;
    public TMP_Text scoretext, tasksuccesstext;
    NPCManagement npcmanager;
    //GameObject currenttile;
    int tileshiftfactor;
    public TMP_Text dialoguetext, questiontext, explaintext, optionAtext, optionBtext, optionCtext, optionDtext;
    public TMP_Text nametext;
    public GameObject optionAButton, optionBButton, optionCButton, optionDButton;
    public GameObject dialoguebox, questionbox;
    int numberOfTiles = 5;
    bool onceComplete = false;
    public float tilerng;
    public enum Level { Bus, MRT };
    public Level level;
    public Tile[] currenttiles;
    CameraController cam;
    public Sprite[] loadingimgs;
    [HideInInspector]public Player player;
    public GameObject upgradeText,boost,taskfailimg;
    ObjectPool objectPool;
    //[SerializeField]Tile starttile;
    // Start is called before the first frame update
    private void Awake() {
        if (PlayerPrefs.GetInt("bool") == 1)
        {
            score = PlayerPrefs.GetInt("score");
            level = (Level)PlayerPrefs.GetInt("Level");
        }
        else
        {
            PlayerPrefs.SetInt("bool",1);
            PlayerPrefs.SetInt("score", 0);
            PlayerPrefs.SetInt("Level", (int)level);
        }
    }
    void Start() {
        objectPool = GetComponent<ObjectPool>();
        if (AudioManager.instance.CheckClip() != AudioManager.instance.levelmusic || !AudioManager.instance.IsPlaying()) {
            // CheckClip() is for when player starts level from level select. the other condition is when level is restarted either from pause screen or game over screen.
            StartCoroutine(AudioManager.instance.SwitchMusic(AudioManager.instance.levelmusic));
        }
        npcmanager = FindObjectOfType<NPCManagement>();
        gameover = false;
        gameoverscreen.SetActive(false);
        cam = FindObjectOfType<CameraController>();
        player = FindObjectOfType<Player>();
        if (level == Level.Bus) {
            cam.transform.position = cam.originalposition.position;
            cam.lookOffset = cam.defaultoffset;
            for (int i = 0; i < tiles.Length; i++) { // tiles is the array of tiles that are to be spawned for the level
                tiles[i] = bustiles[i];
            }
            Spawn(8);
        } 
        else {
            cam.lookOffset = cam.trainoffset;
            for (int i = 0; i < tiles.Length; i++) { // mrt is the single mrt prefab that we are using
                tiles[i] = mrt;
            }
            tileindex = 0; // in bus level, tileindex is randomised for the purposes of tile randomisation. but in train level this not necessary as all tiles are the same. so tileindex does not matter and is just set to 0.
            Spawn(8);
            foreach(var i in FindObjectsOfType<Tile>()) { // finds closest train tile and moves player there. might get rid of this in the future.
                float closest = 999;
                if(Vector3.Distance(transform.position, i.transform.position) < closest) {
                    closest = Vector3.Distance(transform.position, i.transform.position);
                }
                if(Vector3.Distance(transform.position, i.transform.position) == closest) {
                    player.transform.position = i.transform.Find("Player Start Point").position;
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {
        //print("Level:" + level);
        //print("Shift:" + tileshiftfactor);
        if (gameover) {
            gameoverscreen.SetActive(true);
            AudioManager.instance.StopMusic();
        }
        scoretext.text = "Score:" + score;
        ShowFPS(FPS.GetCurrentFPS().ToString());
        if (npcmanager.myNPC != null) {
            if (npcmanager.myNPC.tasksuccess == NPC.Task.Fail) {
                tasksuccesstext.text = "Task failed!";
                taskfailimg.SetActive(true);
            } 
            else if (npcmanager.myNPC.tasksuccess == NPC.Task.Success) {
                tasksuccesstext.text = "Task success!";
                taskfailimg.SetActive(false);
            }
        }
        if (taskcompletescreen.activeSelf && onceComplete == false)
        {
            onceComplete = true;
            StartCoroutine(DisableTaskScreen());
        }
        currenttiles = FindObjectsOfType<Tile>();
        if (currenttiles.Length == 1 || level == Level.MRT) {
            tileshiftfactor = 0; // in mrt level, tileshiftfactor is 0 because the size of the train already shifts them properly (i think)(trust bro) 
        } 
        else if (level == Level.Bus) {
            tileshiftfactor = 21; //  tileshiftfactor spawns tiles 21 units ahead because when player enters trigger, there are 3 tiles in front. each tile is 7 units long on the x-axis
        }
    }
    public void RestartLevel() {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
        Initalize();
    }
    IEnumerator DisableTaskScreen() {
        yield return new WaitForSeconds(1.3f);
        taskcompletescreen.SetActive(false);
        onceComplete = false;
    }
    public void MoveToTrain() {

        print("yes2");
        level = Level.MRT;
        SaveData();
        LoadScene(2); // mrt level
        loadingscreen.SetActive(true);
        loadingscreen.GetComponent<Image>().sprite = loadingimgs[UnityEngine.Random.Range(0, loadingimgs.Length)];
    }
    public void MoveToBus() {

        print("yuh");
        level = Level.Bus;
        SaveData();
        loadingscreen.SetActive(true);
        loadingscreen.GetComponent<Image>().sprite = loadingimgs[UnityEngine.Random.Range(0, loadingimgs.Length)];
        LoadScene(1); // bus level
    }
    public void Spawn(int amount) {
        for (int x = 0; x < amount; x++) { // spawn amount tiles at a time
            Tile mytile;
            if (amount == 1) {
                x = numberOfTiles;
            }
            if (level == Level.Bus) RandomTile(); // randomise tiles in bus level, not needed in mrt level since all tiles are the same
            mytile = tiles[tileindex].GetComponent<Tile>(); // tileindex is randomised by RandomTile()
            if (level == Level.Bus) {
                // spawn tile at spawn point + size of tile * order that tile was spawned + shift
                objectPool.SpawnFromPool(tiles[tileindex].name, mytile.spawnpt.position + new Vector3(7 * x, 0, 0) + new Vector3(tileshiftfactor, 0, 0));
                //ObjectPool.Spawn(tiles[tileindex], mytile.spawnpt.position + new Vector3(7 * x, 0, 0) + new Vector3(tileshiftfactor, 0, 0), Quaternion.identity);
                //Instantiate(tiles[tileindex], mytile.spawnpt.position + new Vector3(7 * x, 0, 0) + new Vector3(tileshiftfactor, 0, 0), Quaternion.identity);
            } 
            else {
                print("trains");
                objectPool.SpawnFromPool(tiles[tileindex].name, mytile.spawnpt.position + new Vector3(26.5f * x, 0, 0) + new Vector3(tileshiftfactor, 0, 0));
                //ObjectPool.Spawn(tiles[tileindex], mytile.spawnpt.position + new Vector3(26.5f * x, 0, 0) + new Vector3(tileshiftfactor, 0, 0), Quaternion.identity);
                //Instantiate(tiles[tileindex], mytile.spawnpt.position + new Vector3(26.5f * x, 0, 0) + new Vector3(tileshiftfactor, 0, 0), Quaternion.identity);
            }
            if (amount == 1) numberOfTiles += 1;
        }
    }
    void SaveData()
    {
        PlayerPrefs.SetInt("bool", 1);
        PlayerPrefs.SetFloat("energy", player.energy);
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.SetInt("Level", (int)level);
        PlayerPrefs.SetFloat("Invincibility Time", player.originalInvincibleTime);
        PlayerPrefs.SetFloat("Energy Gain", player.energygain);
    }

    public void Initalize()
    {
        PlayerPrefs.SetInt("bool", 1);
        PlayerPrefs.SetFloat("energy", 100);
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetFloat("Energy Gain", 10f);
        PlayerPrefs.SetFloat("Invincibility Time", 10f);
        PlayerPrefs.SetInt("Level", (int)Level.Bus);
        LoadData();
    }
    void LoadData()
    {
        player.energy = PlayerPrefs.GetFloat("energy");
        player.energygain = PlayerPrefs.GetFloat("Energy Gain");
        player.originalInvincibleTime = PlayerPrefs.GetFloat("Invincibility Time");
        score = PlayerPrefs.GetInt("score");
        level = (Level)PlayerPrefs.GetInt("Level");
    }
    void RandomTile() {
        tilerng = UnityEngine.Random.Range(0f, 1f);
        if (tilerng <= .5f && tilerng > 0) { // 50% chance of getting a road tile
            foreach (var i in tiles) {
                if (i.GetComponent<RoadTile>()) tileindex = Array.IndexOf(tiles, i); // get index of road tile in tiles array
            }
        } else {
            List<int> legalindexes = new List<int>(); // indexes that are not the index of the road tile
            for (int i = 0; i < tiles.Length; i++) {
                if (!tiles[i].GetComponent<RoadTile>()) legalindexes.Add(i);
            }
            System.Random random = new System.Random();
            tileindex = legalindexes[random.Next(0, legalindexes.Count)]; // gets random index
        }
    }

}
/*
public static class SaveSystem {
    static string path = Application.persistentDataPath + "/SaveData.save"; // actual save file, might not work on webgl
    public static void Initialise(LevelManager.Level level) { // honestly this might not need a param since it's only used for bus level but whatever
        BinaryFormatter formatter = new BinaryFormatter(); // saves data as binary
        FileStream stream = new FileStream(path,FileMode.Create);
        SaveData data = new SaveData(level);
        formatter.Serialize(stream, data); // turns data into binary
        stream.Close();
    }
    public static void Save(LevelManager levelManager) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        SaveData data = new SaveData(levelManager);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static SaveData Load() {
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData; // turns binary into SaveData class (i totally know what im talking about bro trust)
            stream.Close();
            return data;
        } 
        else {
            Debug.Log("Save file not found in " + path);
            Initialise(LevelManager.Level.Bus); // initialise data if save file doesnt exist
            return null;
        }
    }
}
[System.Serializable]
public class SaveData {
    public int score;
    public float energy;
    public LevelManager.Level level;
    public SaveData(LevelManager.Level lvl) { // initial values. will have to figure out a way of doing this that doesn't require hard-coding values
        score = 0;
        energy = 100;
        level = lvl;
    }
    public SaveData(LevelManager levelManager) { // saves current values
        score = levelManager.score;
        energy = levelManager.player.energy;
        level = levelManager.level;
    }
}*/
