using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public Transform spawnpt;
    public GameObject[] NPC;
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
    [SerializeField]Transform[] destinationSpawnPoints;
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
        else {
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
            if (destinationSpawnPoints.Length > 0) {
                GameObject g = Instantiate(destinations[Random.Range(0, destinations.Length)], destinationSpawnPoints[Random.Range(0, destinationSpawnPoints.Length)].position, Quaternion.identity);
                g.SetActive(true);
                g.transform.SetParent(transform);
            }
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
        int NPCIndex = (rng > .5f && rng < 1) ? 0 : 1;
        GameObject go = objectPool.SpawnFromPool(NPC[NPCIndex].name, new Vector3(spawnpt.position.x, spawnpt.position.y + NPC[NPCIndex].GetComponent<NPC>().spawnYOffset, spawnpt.position.z));
        if (gameObject.CompareTag("Train")) go.transform.localScale = new Vector3(.8f,.8f,.8f);
        //if (rng > .5f && rng < 1) {
        //    if (gameObject.CompareTag("Train")) {
        //        if(NPC.Length > 0) {
        //            if(NPCSpawnPoints.Length > 0) {
        //                Transform spawnpt = NPCSpawnPoints[Random.Range(0, NPCSpawnPoints.Length)];
        //                GameObject go = objectPool.SpawnFromPool(NPC[0].name, new Vector3(spawnpt.position.x, spawnpt.position.y + NPC[0].GetComponent<NPC>().spawnYOffset, spawnpt.position.z));
        //                go.transform.localScale = new Vector3(.8f, .8f, .8f);
        //            }
        //        }
        //        if(destinations.Length > 0) {
        //            Instantiate(destinations[Random.Range(0, destinations.Length)], destinationSpawnPoints[Random.Range(0, destinationSpawnPoints.Length)].position, Quaternion.identity);
        //        }
        //    } 
        //    else {
        //        if(destinations.Length > 0)objectPool.SpawnFromPool(NPC[0].name, new Vector3(spawnpt.position.x, spawnpt.position.y + NPC[0].GetComponent<NPC>().spawnYOffset, spawnpt.position.z));
        //    }
        //} 
        //else {
        //    if (gameObject.CompareTag("Train")) {
        //        if (NPC.Length > 0) {
        //            if(NPCSpawnPoints.Length > 0) {
        //                Transform spawnpt = NPCSpawnPoints[Random.Range(0, NPCSpawnPoints.Length)];
        //                GameObject go = objectPool.SpawnFromPool(NPC[1].name, new Vector3(spawnpt.position.x, spawnpt.position.y + NPC[1].GetComponent<NPC>().spawnYOffset, spawnpt.position.z));
        //                go.transform.localScale = new Vector3(.8f, .8f, .8f);
        //            }
        //        }
        //        if (destinations.Length > 0) Instantiate(destinations[Random.Range(0, destinations.Length)], destinationSpawnPoints[Random.Range(0, destinationSpawnPoints.Length)].position, Quaternion.identity);
        //    } 
        //    else {
        //        if(destinations.Length > 0)objectPool.SpawnFromPool(NPC[1].name, new Vector3(spawnpt.position.x, spawnpt.position.y + NPC[1].GetComponent<NPC>().spawnYOffset, spawnpt.position.z));
        //    }
        //}
    }

    // Update is called once per frame
    protected virtual void Update() {
       
        //if(NPC.Length == 1) {
        //    if (NPC[0] == null) return;
        //    if (rng > 0.5f && rng <= 1) NPC[0].SetActive(true);
        //    else NPC[0].SetActive(false);
        //}
        
        //if (NPC.Length > 1) {
        //    if (NPC[0] == null || NPC[1] == null) return;
        //    if (rng > 0.5f && rng <= 1) { // 50% chance of npc 0
        //        NPC[0].SetActive(true);
        //        NPC[1].SetActive(false);
        //    } else if (rng <= 0.5f && rng > 0) { // 50% chance of NPC 1
        //        NPC[0].SetActive(false);
        //        NPC[1].SetActive(true);
        //        //foreach (var i in NPC) {
        //        //    if(i != null)i.gameObject.SetActive(true);
        //        //    //if (!i.followplayer) i.transform.localPosition = i.startpos;
        //        //}
        //    }
        //}

        //for(int i = 0; i < lanes.Length; i++) { // position at which player snaps to changes as they move
        //    lanes[i] = new Vector3(levelManager.player.transform.position.x, lanes[i].y, lanes[i].z);
        //}
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
