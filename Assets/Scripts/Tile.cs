using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public Transform spawnpt;
    //public GameObject street;
    //bool once=true;
    GameObject NPC;
    [SerializeField]GameObject[] buildings;
    protected LevelManager levelManager;
    //public Vector3 spawnoffset;
    protected float rng;
    // Start is called before the first frame update
    protected virtual void Start() {
        NPC = GetComponentInChildren<NPC>(true).gameObject;
        levelManager = FindObjectOfType<LevelManager>();
        rng = Random.Range(0f, 1f);
        //Destroy(Instantiate(tile, spawnpt.position, spawnpt.rotation), 2f);
        //for (int i = 0; i < 10; i++) {
        //    Instantiate(tile, spawnpt.position, spawnpt.rotation);
        //}
    }

    // Update is called once per frame
    void Update() {
        if (rng == 0) { // no NPC or buildings
            NPC.SetActive(false);
            foreach (GameObject i in buildings) i.SetActive(false);
        } 
        else if (rng <= 0.25f && rng > 0) { // 25% chance of NPC
            NPC.SetActive(true);
            //Instantiate(street, spawnpt.position, spawnpt.rotation);
            if(!NPC.GetComponent<NPC>().followplayer)NPC.transform.localPosition = NPC.GetComponent<NPC>().startpos;
        } 
        else if (0.25f < rng && rng <= 0.75f) { // 50% chance of buildings
            foreach (GameObject i in buildings) i.SetActive(true);
            NPC.SetActive(false);
        }
    }

    protected void OnTriggerEnter(Collider other) {
        //SpawnTiles[] currenttiles = FindObjectsOfType<SpawnTiles>();
        //print(currenttiles.Length);
        //if(currenttiles.Length % 4 == 0) {
        //    shiftfactor = 21;
        //}
        //spawn tiles at designated area by either calling the manager or spawning the tiles itself
        if (other.GetComponent<Player>())//only the player collision will spawn the tile
        {
            levelManager.Spawn(1,this);
            print(spawnpt);
            //print(spawnoffset);
            //if (once)
            //{

            //    once = false;
            //    //if(!FindObjectOfType<CameraController>().NPC)Destroy(go, 100);
            //}
        }
    }

   

    protected void OnTriggerExit(Collider other) {
        //NPC.transform.position = NPC.GetComponent<NPC>().startpos;
        if (other.GetComponent<Player>()) Destroy(gameObject);
    }
}
