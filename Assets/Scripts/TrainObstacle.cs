using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainObstacle : Interactable
{
    public GameObject moveNPC;
    public Transform NPClocation;
    public GameObject[] NPCGroup;
    [SerializeField] GameObject[] NPCs;
    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        SetNPCs();
    }
    public void MoveNPC() {
        moveNPC.transform.position = NPClocation.position;
        player.inputtext.SetActive(false);
    }
    protected override void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Player>()) {
            player.inputtext.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other) {
        if (other.GetComponent<Player>() && Input.GetKeyDown(KeyCode.F)) {
            MoveNPC();
            player.inputtext.SetActive(false);
        }
    }
    public void SetNPCs() {
        for (int i = 0; i < NPCGroup.Length; i++) {
            NPCGroup[i] = Instantiate(NPCs[Random.Range(0, NPCs.Length)], NPCGroup[i].transform.position, Quaternion.Euler(0, -90, 0));
            NPCGroup[i].transform.localScale *= 20;
            NPCGroup[i].transform.SetParent(transform);
            moveNPC = NPCGroup[1];
        }
    }
    protected override void Update() {
       
    }
    // Update is called once per frame

}
