using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainObstacle : Interactable
{
    public GameObject moveNPC;
    public Transform NPClocation;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        npcmanager = FindObjectOfType<NPCManagement>();
    }
    public void MoveNPC() {
        moveNPC.transform.position = NPClocation.transform.position;
    }
    // Update is called once per frame
    
}
