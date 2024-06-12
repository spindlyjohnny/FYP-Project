using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Interactable : MonoBehaviour
{
    public Player player;
    protected NPCManagement npcmanager;
    protected LevelManager levelManager;
    //public string location;
    public bool target = false;
    float radius;
    // Start is called before the first frame update
    protected void Start()
    {
        player = FindObjectOfType<Player>();
        //inputtext.SetActive(false);
        npcmanager = FindObjectOfType<NPCManagement>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Collider[] detector;
        if (levelManager.level == LevelManager.Level.Bus) {
            radius = 1f;
        } 
        else {
            radius = 0.4f;
        }
        if (npcmanager.myNPC != null) {
            detector = Physics.OverlapSphere(transform.position, radius); // detects NPC
            for (int i = 0; i < detector.Length; i++) {
                if (detector[i] == null) return;
                if (npcmanager.myNPC.gameObject == null) target = false;
                if (detector[i].gameObject == npcmanager.myNPC.gameObject) target = true;
                print("target:" + target);
            }
            if (GetComponentInParent<RoadTile>()) { // sets NPC street to gameobject if it's a roadtile
                npcmanager.myNPC.street = GetComponentInParent<RoadTile>();
            }
            if (target /*&& npcmanager.myNPC.sub == npcmanager.myNPC.temp/*!gameObject.CompareTag("Finish") && !GetComponent<TrainObstacle>()*/) {
                player.inputtext.SetActive(true);
                //transform.SetParent(null);
                if (Input.GetKeyDown(KeyCode.F) && gameObject.CompareTag("Transition")) npcmanager.myNPC.Transitioninator();
                //levelManager.Spawn(1);
            } 
        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    protected virtual void OnTriggerEnter(Collider other) {
        
    }
    protected void OnTriggerExit(Collider other) {
        if (other.GetComponent<Player>()) player.inputtext.SetActive(false);
    }
}
