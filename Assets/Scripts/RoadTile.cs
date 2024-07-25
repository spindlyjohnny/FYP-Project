using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTile : Tile
{
    public Bus bus;
    public Transform campos;
    public GameObject station;
    // Start is called before the first frame update
    protected override void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        npcmanager = FindObjectOfType<NPCManagement>();
        bus = GetComponentInChildren<Bus>(true);
        campos = bus.transform.Find("Cam Position");
        lanes[0].z = 1.3f;
        lanes[1].z = 0;
        lanes[2].z = -1.3f;
    }

    // Update is called once per frame
    void Update() {
        for (int i = 0; i < lanes.Length; i++) { // position at which player snaps to changes as they move
            lanes[i] = new Vector3(levelManager.player.transform.position.x, lanes[i].y, lanes[i].z);
        }
    }
}
