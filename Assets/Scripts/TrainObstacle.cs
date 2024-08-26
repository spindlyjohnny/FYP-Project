using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrainObstacle : Interactable
{
    public GameObject moveNPC;
    public Transform NPClocation;
    public List<GameObject> NPCGroup;
    [SerializeField] GameObject[] NPCs;
    GameObject[] npcarray = new GameObject[3];
    public GameObject excuseMeText;
    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        //SetNPCs();
    }
    public void MoveNPC() {
        moveNPC.transform.position = NPClocation.position;
        player.inputtext.SetActive(false);
    }
    protected override void OnTriggerEnter(Collider other) {
        //if (npcarray[0] == null) return;
        if (other.GetComponent<Player>()) {
            touchingPlayer = true;
            player.canMove = false;
            player.inputtext.SetActive(true);
            excuseMeText.SetActive(true);
        }
    }
    private void OnEnable() {
        SetNPCs();
    }
    private void OnDisable() {
        foreach (var i in npcarray) {
            Destroy(i);
        }
        Array.Clear(npcarray,0,npcarray.Length);
    }
    public void SetNPCs() {
        //NPCGroup = new List<GameObject>(3);
        //if (npcCount == 3) return;
        for (int i = 0; i < NPCGroup.Count; i++) {
            //NPCGroup.Insert(i, Instantiate(NPCs[UnityEngine.Random.Range(0, NPCs.Length)], NPCGroup[i].transform.position, Quaternion.Euler(0, -90, 0)));
            npcarray[i] = Instantiate(NPCs[UnityEngine.Random.Range(0, NPCs.Length)], NPCGroup[i].transform.position, Quaternion.Euler(0, -90, 0));
            npcarray[i].transform.localScale *= 25;
            npcarray[i].transform.SetParent(transform);
            moveNPC = npcarray[1];
            foreach(var x in npcarray[i].GetComponentsInChildren<Animator>())x.enabled = false;
        }
    }
    protected override void Update() {
        if (touchingPlayer && Input.GetKeyDown(KeyCode.F)) {
            MoveNPC();
            player.inputtext.SetActive(false);
            excuseMeText.SetActive(false);
            player.canMove = true;
        }
    }
    // Update is called once per frame

}
