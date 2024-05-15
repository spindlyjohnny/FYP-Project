using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTile : Tile
{
    public Bus bus;
    public Transform campos;
    NPCManagement npcmanager;
    // Start is called before the first frame update
    protected override void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        npcmanager = FindObjectOfType<NPCManagement>();
        bus = GetComponentInChildren<Bus>(true);
        campos = bus.transform.Find("Cam Position");
    }

    // Update is called once per frame
    void Update()
    {
        /*if (npcmanager.myNPC != null && npcmanager.myNPC.tasksuccess == NPC.Task.Success) {
            bus.gameObject.SetActive(true);
        }*/
    }
}
