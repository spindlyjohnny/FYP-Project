using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTile : Tile
{
    GameObject bus;
    NPCManagement npcmanager;
    // Start is called before the first frame update
    protected override void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        npcmanager = FindObjectOfType<NPCManagement>();
        bus = GetComponentInChildren<Obstacle>(true).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //if(npcmanager.myNPC.tasksuccess == npcmanager.myNPC.Task.Success)
        //  bus.SetActive(true);
    }
}
