using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public bool stopSpawningNpc=false;
    public Transform spawnpt;
    public GameObject[] NPC;
    public GameObject spawnedNpc;
    protected LevelManager levelManager;
    protected NPCManagement npcmanager;
    //public Vector3 spawnoffset;
    protected float rng;
    bool once = false;
    public Vector3[] lanes; // order of array should be like so: left, middle, right
    public Transform[] NPCSpawnPoints;
    [SerializeField] bool hasNPC;
    ObjectPool objectPool;
    [SerializeField]GameObject[] L1Destinations, L2Destinations, L3Destinations;
    [SerializeField]GameObject[] destinations;
    //[SerializeField]Transform[] destinationSpawnPoints;
    int NPCIndex;
    public bool randomNPCs;
    // Start is called before the first frame update
    protected virtual void Start() {
        //NPC = GetComponentsInChildren<NPC>(true);
        levelManager = FindObjectOfType<LevelManager>();
        npcmanager = FindObjectOfType<NPCManagement>();
        rng = Random.Range(0f, 1f);
        objectPool = GetComponent<ObjectPool>();
        //lanes[0].z = 1.3f;
        //lanes[1].z = 0;
        //lanes[2].z = -1.3f;
        if (!gameObject.CompareTag("Train")) {
            lanes[0].z = 1.3f;
            lanes[1].z = 0;
            lanes[2].z = -1.3f;
        }
        switch (LevelManager.levelNum) {
            case LevelManager.LevelNum.Level1:
                destinations = L1Destinations;
                break;
            case LevelManager.LevelNum.Level2:
                destinations = L2Destinations;
                break;
            case LevelManager.LevelNum.Level3:
                destinations = L3Destinations;
                break;
        }
        if (!hasNPC) return;
        switch (LevelManager.levelNum) {
            case LevelManager.LevelNum.Level1:
                NPC = levelManager.level1NPC;
                break;
            case LevelManager.LevelNum.Level2:
                NPC = levelManager.level2NPC;
                break;
            case LevelManager.LevelNum.Level3:
                NPC = levelManager.level3NPC;
                break;
        }
        Transform spawnpt = NPCSpawnPoints[Random.Range(0, NPCSpawnPoints.Length)];
        if(LevelManager.levelNum != LevelManager.LevelNum.Level3) {
            NPCIndex = (rng > .5f && rng <= 1) ? 0 : 1;
        } 
        else {
            NPCIndex = (rng > .33f && rng <= .66f) ? 0 : (rng > .66f && rng <= 1) ? 1 : 2;
        }
        spawnedNpc = objectPool.SpawnFromPool(NPC[NPCIndex].name, new Vector3(spawnpt.position.x, spawnpt.position.y, spawnpt.position.z));
        if (stopSpawningNpc==true) spawnedNpc.SetActive(false);
        if (gameObject.CompareTag("Train") & spawnedNpc != null) spawnedNpc.transform.localScale = new Vector3(.8f, .8f, .8f);

    }
    // Update is called once per frame
    private void Update() {
        if (destinations.Length > 0) {
            if(npcmanager.myNPC != null) {
                foreach(var i in destinations) {
                    if(i.GetComponent<Interactable>().location == (int)npcmanager.myNPC.dialogueData.dialogueQuestions[npcmanager.myNPC.qnindex].outcomeLocation) {
                        i.SetActive(true);
                    }
                }
            }
            //if (randomNPCs) {
            //    destinations[Random.Range(0, destinations.Length)].SetActive(true);
            //} 
            //else {
            //    foreach (var i in destinations) i.SetActive(true);
            //}
            //if(levelManager.level == LevelManager.Level.BusInterior) {
            //    foreach (var i in destinations) i.SetActive(true);
            //} 
            //else {
            //    destinations[Random.Range(0, destinations.Length)].SetActive(true);
            //}
        }
    }
    protected void OnTriggerEnter(Collider other) {
        if (once == true) return;
        //spawn tiles at designated area by either calling the manager or spawning the tiles itself
        if (other.GetComponent<Player>())//only the player collision will spawn the tile
        {
            once = true;
            float size = levelManager.level == LevelManager.Level.Bus ? 13 : 30/*26.5f*/;
            levelManager.Spawn(1,size);
            if (GetComponentInChildren<TrainObstacle>()) GetComponentInChildren<TrainObstacle>().SetNPCs();
        }
    }

}
