using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Video;

public class LevelManager : SceneLoader {
    public bool gameover;
    public GameObject gameoverscreen, taskcompletescreenResponse, taskcompletescreen, fadeIn, dialoguescreen,guidebox;
    public Video loadingScreen;
    public Slider energyslider;
    public GameObject[] tiles;
    [SerializeField] GameObject[] bustiles,level1Bus,level2Bus,level3Bus;
    public GameObject[] mrt;
    public int score; 
    public int tileindex,tileAmount;
    public TMP_Text scoretext, tasksuccessResponsetext,tasksuccesstext,finalScoreText, guideText;
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
    public enum Level { Bus,BusInterior, MRT };
    public enum LevelNum { Level1 = 1,Level2 = 2,Level3 = 3}
    public Level level;
    public static LevelNum levelNum;
    public Tile[] currenttiles;
    CameraController cam;
    //public Sprite[] loadingimgs;
    [HideInInspector]public Player player;
    public GameObject upgradeText, boost,boostResponse;
    public Image taskCompleteImgResponse, taskCompleteImg;
    ObjectPool objectPool;
    public Image npcAvatar,pauseImg,failImg;
    public GameObject[] level1NPC, level2NPC, level3NPC;
    public int tilesSpawned;
    public Sprite[] taskCompletionPanelSprites;
    public bool pausing;
    public GameObject[] players;
    // Start is called before the first frame update
    private void Awake() {
        if (PlayerPrefs.HasKey("Level Num"))
        {
            print("yes");
            score = PlayerPrefs.GetInt("score");
            levelNum = (LevelNum)PlayerPrefs.GetInt("Level Num");
            //level = (Level)PlayerPrefs.GetInt("Level");
        }
        else
        {
            print("yes");
            PlayerPrefs.SetInt("score", 0);
            PlayerPrefs.SetInt("Level", (int)level);
            PlayerPrefs.SetInt("Level Num", (int)levelNum);
            levelNum = (LevelNum)1;
        }
        for (int i = 0; i < players.Length; i++) {
            if (i == PlayerPrefs.GetInt("Player", 0)) {
                players[i].SetActive(true);
            } 
            else {
                players[i].SetActive(false);
            }
        }
    }
    void Start() {
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            PlayerPrefs.SetInt("Tutorial", 0);//0 is  false and 1 is true            
        }
        tilesSpawned = 0;
        objectPool = GetComponent<ObjectPool>();
        FadeIn();
        if (AudioManager.instance.CheckClip() != AudioManager.instance.levelmusic || !AudioManager.instance.IsPlaying()) {
            // CheckClip() is for when player starts level from level select. the other condition is when level is restarted either from pause screen or game over screen.
            StartCoroutine(AudioManager.instance.SwitchMusic(AudioManager.instance.levelmusic));
        }
        npcmanager = FindObjectOfType<NPCManagement>();
        gameover = false;
        gameoverscreen.SetActive(false);
        cam = FindObjectOfType<CameraController>();
        player = FindObjectOfType<Player>();
        switch (levelNum) {
            case LevelNum.Level1:
                bustiles = level1Bus;
                break;
            case LevelNum.Level2:
                bustiles = level2Bus;
                break;
            case LevelNum.Level3:
                bustiles = level3Bus;
                break;
        }
        if (level == Level.Bus) {
            cam.transform.position = cam.originalposition.position;
            cam.lookOffset = cam.defaultoffset;
            for (int i = 0; i < tiles.Length; i++) { // tiles is the array of tiles that are to be spawned for the level
                tiles[i] = bustiles[i];
            }
            Spawn(8,13);
        } 
        else if(level == Level.MRT){
            //cam.lookOffset = cam.interioroffset;
            for (int i = 0; i < tiles.Length; i++) {
                tiles[i] = mrt[i];
            }
            Spawn(8,30f);
        } 
        else {
            cam.lookOffset = cam.interioroffset;
        }
        taskCompletionPanelSprites[0] = player.loseSprite;
        pauseImg.sprite = player.pauseSprite;
        failImg.sprite = player.loseSprite;
        pausing = false;
    }

    // Update is called once per frame
    void Update() {
        if (gameover) {
            gameoverscreen.SetActive(true);
            finalScoreText.text = "Score: "+score;
            AudioManager.instance.StopMusic();
        }
        scoretext.text = score.ToString();
        FPSCounter(FPS.GetCurrentFPS().ToString());
        if (npcmanager.myNPC != null) {
            if (npcmanager.myNPC.tasksuccess == NPC.Task.Fail) {
                tasksuccessResponsetext.text = "Task failed!";
                tasksuccesstext.text = "Task failed!";
                //taskCompleteImg.SetActive(true);
            } 
            else if (npcmanager.myNPC.tasksuccess == NPC.Task.Success) {
                tasksuccessResponsetext.text = "Task success!";
                tasksuccesstext.text = "Task success!";
                //taskCompleteImg.SetActive(false);
            }
        }
        currenttiles = FindObjectsOfType<Tile>();//why is this in update(), why not move to start()?
        if (currenttiles.Length == 1) {
            tileshiftfactor = 0; // in mrt level, tileshiftfactor is 0 because the size of the train already shifts them properly (i think)(trust bro) (me no trust)
        }
        else if(level == Level.MRT) {
            tileshiftfactor = 60; // lmao at line 149 comment
        }
        else if (level == Level.Bus) {
            tileshiftfactor = 26; //  tileshiftfactor spawns tiles 21 units ahead because when player enters trigger, there are 3 tiles in front. each tile is 7 units long on the x-axis
        }
        if (Input.GetKeyDown(KeyCode.Tab)) ShowFPS();

        //RenderSettings.skybox.SetFloatArray("_Rotation",new List<float>() {90,0,0 });
    }
    //public void RestartLevel() {
    //    LoadScene(SceneManager.GetActiveScene().buildIndex);
    //    Initalize();
    //}
    public void DisableTaskScreen() {
        if(npcmanager.myGeneral == null)player.canMove = true;
        taskcompletescreenResponse.SetActive(false);
        taskcompletescreen.SetActive(false);
        Time.timeScale=1;
    }
    public IEnumerator Move(int index,Level lvl) { // transition between levels
        // index is the buildIndex of the level that you are going to, lvl is the Level enum value of the level that you are going to
        level = lvl;
        loadingScreen.gameObject.SetActive(true);
        if (lvl == Level.Bus || lvl == Level.BusInterior) {
            loadingScreen.videoName = "Bus_Loading_Transition.mp4";
        } 
        else {
            loadingScreen.videoName = "MRT_Loading_Transition.mp4";
        }
        loadingScreen.PlayVideo();
        yield return new WaitForSeconds(lvl == Level.MRT ? 6f : 8f);
        LoadScene(index,true);
        SaveData();
        PlayerPrefs.Save();
    }
    public void Spawn(int amount,float size) {
        for (int x = 0; x < amount; x++) { // spawn amount tiles at a time
            Tile mytile;
            if (amount == 1) {
                x = numberOfTiles;
            }
            /*if(level != Level.MRT)tileindex = 8;
            else*/ RandomTile();
            TutorialTile();
            mytile = tiles[tileindex].GetComponent<Tile>(); // tileindex is randomised by RandomTile()
            objectPool.Remove();
            GameObject temp=objectPool.SpawnFromPool(tiles[tileindex].name, mytile.spawnpt.position + new Vector3(size * x, 0, 0) + new Vector3(tileshiftfactor, 0, 0));
            print("x value: " + x);
            tilesSpawned++;
            print("Tiles spawned: " + tilesSpawned);
            print("num tiles: "+numberOfTiles);
            //print("Tile position: "+temp.transform.position);
            if (temp!= null)
            {
                if (temp.GetComponent<Tile>().spawnedNpc != null)
                {
                    temp.GetComponent<Tile>().spawnedNpc.SetActive(true);
                }
               Activate(temp.GetComponent<Tile>());
            }            
            
            //numTiles += 1;
            if (amount == 1) numberOfTiles += 1;
        }
    }
    public void Pausing() {
        pausing = true;
    }
    public void UnPausing() {
        pausing = false;
    }
    void SaveData()
    {
        PlayerPrefs.SetInt("bool", 1);
        PlayerPrefs.SetFloat("energy", player.energy);
        PlayerPrefs.SetInt("score", score);
        //PlayerPrefs.SetInt("Level", (int)level);
        PlayerPrefs.SetFloat("Invincibility Time", player.originalInvincibleTime);
        PlayerPrefs.SetFloat("Max Energy", player.maxenergy);
        PlayerPrefs.SetInt("Level Num",(int)levelNum);
        PlayerPrefs.Save();
    }

    public void Initalize()
    {
        PlayerPrefs.SetInt("bool", 1);
        PlayerPrefs.SetFloat("energy", 100);
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetFloat("Max Energy", 100);
        PlayerPrefs.SetFloat("Invincibility Time", 5f);
        //PlayerPrefs.SetInt("Level Num", (int)LevelNum.Level1);
        PlayerPrefs.SetInt("Level", (int)Level.Bus);
        LoadData();
        PlayerPrefs.Save();
    }
    void LoadData()
    {
        player.energy = PlayerPrefs.GetFloat("energy");
        player.maxenergy = PlayerPrefs.GetFloat("Max Energy");
        player.originalInvincibleTime = PlayerPrefs.GetFloat("Invincibility Time");
        score = PlayerPrefs.GetInt("score");
        level = (Level)PlayerPrefs.GetInt("Level");
        levelNum = (LevelNum)PlayerPrefs.GetInt("Level Num");
        PlayerPrefs.Save();
    }
    void RandomTile() {
        //if (Mathf.FloorToInt(player.distTravelled.magnitude) % 50 == 0 && player.distTravelled.magnitude > 0) {
        //    foreach (var i in tiles) {
        //        if (i.CompareTag("Transition")) tileindex = Array.IndexOf(tiles, i);
        //     }
        //} 
        tilerng = UnityEngine.Random.Range(0f, 1f);
        //print(tilerng);
        if (tilerng > .85f && tilerng <= 1f && tilesSpawned >= 15) {
            foreach (var i in tiles) {
                if (i.CompareTag("Transition")) tileindex = Array.IndexOf(tiles, i);
            }
        } 
        else if (tilerng <= .25f && tilerng > 0) { // 25% chance of getting a road tile
            foreach (var i in tiles) {
                if (i.GetComponent<RoadTile>()) tileindex = Array.IndexOf(tiles, i); // get index of road tile in tiles array
            }
        }
        else {
            List<int> legalindexes = new List<int>(); // indexes that are not the index of the road tile or the train station
            for (int i = 0; i < tiles.Length; i++) {
                if (!tiles[i].GetComponent<RoadTile>()) {
                    if (!tiles[i].CompareTag("Transition")) {
                        legalindexes.Add(i);
                    }
                }
                if(!tiles[i].CompareTag("Transition")) {
                    if (!tiles[i].GetComponent<RoadTile>()) {
                        legalindexes.Add(i);
                    }
                }
            }
            System.Random random = new System.Random();
            tileindex = legalindexes[random.Next(0, legalindexes.Count)]; // gets random index
        }
    }
    void TutorialTile()
    {
        tileAmount += 1;
        if (level != Level.Bus) return;
        if (!PlayerPrefs.HasKey("Tutorial")) return;     
        if (PlayerPrefs.GetInt("Tutorial") == 1) return;
        if (tileAmount <= 2)
        {
            tileindex = 6;
        }        
    }
    void Activate(Tile tile)
    {
        if (!PlayerPrefs.HasKey("Tutorial")) return;
        if (PlayerPrefs.GetInt("Tutorial") == 1) return;
        if (tileAmount >= 3) return;
        tile.stopSpawningNpc = true;
    }
    void FadeIn() {
        Image img = fadeIn.GetComponent<Image>();
        img.CrossFadeAlpha(0, .3f, false);
        Destroy(fadeIn, .3f);
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
