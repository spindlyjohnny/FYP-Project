using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public Transform spawnpt;
    public GameObject NPC;
    protected LevelManager levelManager;
    //public Vector3 spawnoffset;
    protected float rng;
    bool once = false;
    // Start is called before the first frame update
    protected virtual void Start() {
        NPC = GetComponentInChildren<NPC>(true).gameObject;
        levelManager = FindObjectOfType<LevelManager>();
        rng = Random.Range(0f, 1f);
    }

    // Update is called once per frame
    protected virtual void Update() {
        if (NPC == null) return;
        if (rng == 0) { // no NPC or buildings
            NPC.SetActive(false);
        } 
        else if (rng <= 0.5f && rng > 0) { // 50% chance of NPC
            NPC.SetActive(true);
            if (!NPC.GetComponent<NPC>().followplayer) NPC.transform.localPosition = NPC.GetComponent<NPC>().startpos;
        }
    }

    protected void OnTriggerEnter(Collider other) {
        if (once == true) return;
        //spawn tiles at designated area by either calling the manager or spawning the tiles itself
        if (other.GetComponent<Player>())//only the player collision will spawn the tile
        {
            once = true;
            levelManager.Spawn(1);
        }
    }

}
