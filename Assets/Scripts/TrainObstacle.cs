using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainObstacle : Interactable
{
    public GameObject moveNPC;
    public Transform NPClocation;
    public GameObject[] NPCGroup;
    [SerializeField] GameObject[] NPCs;
    public GameObject excuseMeText;
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
            touchingPlayer = true;
            player.canMove = false;
            player.inputtext.SetActive(true);
            excuseMeText.SetActive(true);
        }
    }
    public void SetNPCs() {
        for (int i = 0; i < NPCGroup.Length; i++) {
            NPCGroup[i] = Instantiate(NPCs[Random.Range(0, NPCs.Length)], NPCGroup[i].transform.position, Quaternion.Euler(0, -90, 0));
            NPCGroup[i].transform.localScale *= 20;
            NPCGroup[i].transform.SetParent(transform);
            moveNPC = NPCGroup[1];
            foreach(var x in NPCGroup[i].GetComponentsInChildren<Animator>())x.enabled = false;
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
