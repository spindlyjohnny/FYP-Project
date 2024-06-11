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
    bool onceComplete=false, onceLoading = false;
    public float tilerng;
    public enum Level { Bus, MRT };
    public Level level;
    public Tile[] currenttiles;
    CameraController cam;
    public Sprite[] loadingimgs;
    [HideInInspector]public Player player;
    public GameObject upgradeText,boost;
    ObjectPool objectPool;
    //[SerializeField]Tile starttile;
    // Start is called before the first frame update
    private void Awake() {
        if (Application.isEditor) {
            score = SaveSystem.Load().score;
            level = SaveSystem.Load().level;
        } else {
            score = 0;
        }
    }
    void Start() {
        objectPool = GetComponent<ObjectPool>();
        if (AudioManager.instance.CheckClip() != AudioManager.instance.levelmusic || !AudioManager.instance.IsPlaying()) {
            StartCoroutine(AudioManager.instance.SwitchMusic(AudioManager.instance.levelmusic));
        }
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
            //RandomTile();
        } 
        else {
            //cam.transform.position = cam.trainposition.position;
            cam.lookOffset = cam.trainoffset;
            for (int i = 0; i < tiles.Length; i++) {
                tiles[i] = mrt;
            }
            tileindex = 0;
            Spawn(8);
            foreach(var i in FindObjectsOfType<Tile>()) {
                float closest = 999;
                if(Vector3.Distance(transform.position, i.transform.position) < closest) {
                    closest = Vector3.Distance(transform.position, i.transform.position);
                }
                if(Vector3.Distance(transform.position, i.transform.position) == closest) {
                    player.transform.position = i.transform.Find("Player Start Point").position;
                }
            }
            //player.transform.position = tiles[^1].transform.Find("Player Start Point").position;
        }
    }

    // Update is called once per frame
    void Update() {
        print("Level:" + level);
        //tilerng = UnityEngine.Random.Range(0f, 1f);
        //if (level == Level.Bus) RandomTile();
        //else {
        //    for (int i = 0; i < tiles.Length; i++) {
        //        tiles[i] = mrt;
        //    }
        //    tileindex = 0;
        //}
        print("Shift:" + tileshiftfactor);
        if (gameover) {
            gameoverscreen.SetActive(true);
            AudioManager.instance.StopMusic();
        }
        scoretext.text = "Score:" + score;
        if (npcmanager.myNPC != null) {
            if (npcmanager.myNPC.tasksuccess == NPC.Task.Fail) {
                tasksuccesstext.text = "Task failed!";
            } else if (npcmanager.myNPC.tasksuccess == NPC.Task.Success) {
                tasksuccesstext.text = "Task success!";
            }
        }
        if (taskcompletescreen.activeSelf && onceComplete == false)
        {
            onceComplete = true;
            StartCoroutine(DisableTaskScreen());
        }
        //if (loadingscreen.activeSelf && onceLoading == false)
        //{
        //    onceLoading = true;
        //    StartCoroutine(DisableLoadingScreen(2f));
        //}
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

            //ObjectPool.ReturnToPool(currenttiles[currenttiles.Length - 1].gameObject);
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
        onceComplete = false;
    }
    IEnumerator DisableLoadingScreen(float time) {
        yield return new WaitForSeconds(time);
        loadingscreen.SetActive(false);
        onceLoading = false;
    }
    public void MoveToTrain() {

        print("yes2");
        SaveSystem.Save(this);
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
        level = Level.Bus;
        SaveSystem.Save(this);
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
        for (int x = 0; x < amount; x++) { // spawn amount tiles at a time
            Tile mytile;
            if (amount == 1) {
                x = numberOfTiles;
            }
            if (level == Level.Bus) RandomTile();
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
                objectPool.SpawnFromPool(tiles[tileindex].name, mytile.spawnpt.position + new Vector3(7 * x, 0, 0) + new Vector3(tileshiftfactor, 0, 0));
                //ObjectPool.Spawn(tiles[tileindex], mytile.spawnpt.position + new Vector3(7 * x, 0, 0) + new Vector3(tileshiftfactor, 0, 0), Quaternion.identity);
                //Instantiate(tiles[tileindex], mytile.spawnpt.position + new Vector3(7 * x, 0, 0) + new Vector3(tileshiftfactor, 0, 0), Quaternion.identity);
            } 
            else {
                objectPool.SpawnFromPool(tiles[tileindex].name, mytile.spawnpt.position + new Vector3(26.5f * x, 0, 0) + new Vector3(tileshiftfactor, 0, 0));
                //ObjectPool.Spawn(tiles[tileindex], mytile.spawnpt.position + new Vector3(26.5f * x, 0, 0) + new Vector3(tileshiftfactor, 0, 0), Quaternion.identity);
                //Instantiate(tiles[tileindex], mytile.spawnpt.position + new Vector3(26.5f * x, 0, 0) + new Vector3(tileshiftfactor, 0, 0), Quaternion.identity);
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
//public class ObjectPool : MonoBehaviour{
//    public static List<PooledObject> objectPools = new List<PooledObject>();
//    public static GameObject Spawn(GameObject objectToSpawn, Vector3 position, Quaternion rotation) {
//        PooledObject pool = objectPools.Find(p => p.name == objectToSpawn.name); // find pool where name matches name given in param
//        // if pool doesnt exist, create it
//        if(pool == null) {
//            pool = new PooledObject() { name = objectToSpawn.name };
//            objectPools.Add(pool);
//        }
//        // check if there are inactive objects in pool
//        GameObject spawnedObject = pool.inactiveObjects.FirstOrDefault();
//        if(spawnedObject == null) {
//            // if there are no inactive objects
//            spawnedObject = Instantiate(objectToSpawn, position, rotation);
//        } 
//        else {
//            //if there is an inactive object, reactivate it
//            spawnedObject.transform.position = position;
//            spawnedObject.transform.rotation = rotation;
//            pool.inactiveObjects.Remove(spawnedObject);
//            spawnedObject.SetActive(true);
//        }
//        return spawnedObject;
//    }
//    public static void ReturnToPool(GameObject obj) {
//        string name = obj.name.Substring(0, obj.name.Length - 7); // take off '(Clone)' from end of gameObject name
//        PooledObject pool = objectPools.Find(p => p.name == name);
//        if(pool == null) {
//            Debug.LogWarning("Trying to release object that is not pooled:" + obj.name);
//        } 
//        else {
//            obj.SetActive(false);
//            pool.inactiveObjects.Add(obj);
//        }
//    }
//}
//public class PooledObject {
//    public string name;
//    public List<GameObject> inactiveObjects = new List<GameObject>();
//}
public static class SaveSystem {
    static string path = Application.persistentDataPath + "/SaveData.save";
    public static void Initialise() {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path,FileMode.Create);
        SaveData data = new SaveData();
        formatter.Serialize(stream, data);
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
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        } 
        else {
            Debug.Log("Save file not found in " + path);
            Initialise();
            return null;
        }
    }
}
[System.Serializable]
public class SaveData {
    public int score;
    public float energy;
    public LevelManager.Level level;
    public SaveData() { // initial values. will have to figure out a way of doing this that doesn't require hard-coding values
        score = 0;
        energy = 100;
        level = LevelManager.Level.Bus;
    }
    public SaveData(LevelManager levelManager) { // saves current values
        score = levelManager.score;
        energy = levelManager.player.energy;
        level = levelManager.level;
    }
}
