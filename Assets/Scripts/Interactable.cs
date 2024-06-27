using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Interactable : MonoBehaviour
{
    public Player player;
    protected NPCManagement npcmanager;
    protected LevelManager levelManager;
    //public string location;
    //public bool target = false;
    //float radius;
    // Start is called before the first frame update
    protected void Start()
    {
        player = FindObjectOfType<Player>();
        npcmanager = FindObjectOfType<NPCManagement>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Collider[] detector;
        //if (levelManager.level == LevelManager.Level.Bus) {
        //    radius = 1f; // radius of detector
        //} 
        //else {
        //    radius = 0.4f; // radius in train, 1 is too big
        //}
        //if (npcmanager.myNPC != null) {
        //    detector = Physics.OverlapSphere(transform.position, radius); // detects NPC
        //    for (int i = 0; i < detector.Length; i++) {
        //        if (detector[i] == null) return;
        //        if (npcmanager.myNPC.gameObject == null) target = false; // this line kinda doesnt make sense but it just works so i dont touch
        //        if (detector[i].gameObject == npcmanager.myNPC.gameObject) target = true; // target is for checking if the NPC was detected
        //        print("target:" + target);
        //    }
        //    if (GetComponentInParent<RoadTile>()) { // sets NPC street to gameobject if it's a roadtile
        //        npcmanager.myNPC.street = GetComponentInParent<RoadTile>();
        //    }
        //    if (target) {
        //        player.inputtext.SetActive(true);
        //        player.canMove = false;
        //        //transform.SetParent(null);
        //        if (Input.GetKeyDown(KeyCode.F) && gameObject.CompareTag("Transition")) npcmanager.myNPC.Transitioninator(); // transitions only if player presses F on an NPC destination. This is so that tasks that dont transition, dont transition.
        //        //levelManager.Spawn(1);
        //    } 
        //}
    }
    private void OnDrawGizmos() {
        //Gizmos.DrawWireSphere(transform.position, radius);
    }
    private void OnTriggerStay(Collider other) {
        if (gameObject.CompareTag("Transition") && Input.GetKeyDown(KeyCode.F)) {
            if (npcmanager.myNPC != null) npcmanager.myNPC.Transitioninator();
            else levelManager.Move(3, LevelManager.Level.MRT);
        }
    }
    protected virtual void OnTriggerEnter(Collider other) {
        if(npcmanager.myNPC != null && other.GetComponent<Player>()) {
            player.canMove = false;
            player.inputtext.SetActive(true);
            if (GetComponentInParent<RoadTile>()) npcmanager.myNPC.street = GetComponentInParent<RoadTile>();
            //if (gameObject.CompareTag("Transition") && Input.GetKeyDown(KeyCode.F)) npcmanager.myNPC.Transitioninator();
        }
        else if (other.GetComponent<Player>()) {
            player.inputtext.SetActive(true);
        }
    }
    protected void OnTriggerExit(Collider other) {
        if (other.GetComponent<Player>()) player.inputtext.SetActive(false);
    }
}
