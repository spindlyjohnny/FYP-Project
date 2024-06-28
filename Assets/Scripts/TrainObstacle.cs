using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainObstacle : Interactable
{
    public GameObject moveNPC;
    public Transform NPClocation;
    public GameObject[] NPCGroup;
    // Start is called before the first frame update
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
    protected override void Update() {
        
    }
    // Update is called once per frame

}
