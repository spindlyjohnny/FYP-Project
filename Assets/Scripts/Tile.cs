using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public Transform spawnpt;
    public NPC[] NPC;
    protected LevelManager levelManager;
    protected NPCManagement npcmanager;
    //public Vector3 spawnoffset;
    protected float rng;
    bool once = false;
    // Start is called before the first frame update
    protected virtual void Start() {
        NPC = GetComponentsInChildren<NPC>(true);
        levelManager = FindObjectOfType<LevelManager>();
        npcmanager = FindObjectOfType<NPCManagement>();
        rng = Random.Range(0f, 1f);
    }

    // Update is called once per frame
    protected virtual void Update() {
        if (NPC == null) return;
        if (rng == 0) { // no NPC
            foreach(var i in NPC)i.gameObject.SetActive(false);
        } 
        else if (rng <= 0.5f && rng > 0) { // 50% chance of NPC
            foreach (var i in NPC) {
                if(i != null)i.gameObject.SetActive(true);
                if (!i.followplayer) i.transform.localPosition = i.startpos;
            }
            
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
