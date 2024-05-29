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
                //if (GetComponentInParent<RoadTile>()) { // sets NPC street to gameobject if it's a roadtile
                //    npcmanager.myNPC.street = GetComponentInParent<RoadTile>();
                //}
                //else if(detector[i]) // mrt tile.
            }
            if (GetComponentInParent<RoadTile>()) { // sets NPC street to gameobject if it's a roadtile
                npcmanager.myNPC.street = GetComponentInParent<RoadTile>();
            }
            //location = npcmanager.myNPC.temp;
            //print("target && npclocation == location"+ (target && npcmanager.myNPC.npcLocation == location));
            if (target && npcmanager.myNPC.sub == npcmanager.myNPC.temp/*!gameObject.CompareTag("Finish") && !GetComponent<TrainObstacle>()*/) {
                player.inputtext.SetActive(true);
                //transform.SetParent(null);
                if (Input.GetKeyDown(KeyCode.F) && gameObject.CompareTag("Transition")) npcmanager.myNPC.Transitioninator();
                //levelManager.Spawn(1);


            }
        }

        // if npc is touching self
        
        if( Input.GetKeyDown(KeyCode.F) && (gameObject.CompareTag("Train"))) {
            print("yes");
            player.inputtext.SetActive(false);
            FindObjectOfType<Bus>().transitioned = false;
            levelManager.MoveToTrain();
        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    protected virtual void OnTriggerEnter(Collider other) {
        //if (other.GetComponent<Player>()) {
        //    player.inputtext.SetActive(false);
        //}
        //if (target) {
        //    //if (npcmanager.myNPC == null) return;
        //    if (other.GetComponent<Player>() && npcmanager.myNPC.followplayer) player.inputtext.SetActive(true);
        //} 
        //else {
           
        //}
    }
    //IEnumerator Transition() {
    //    levelManager.loadingscreen.SetActive(true);
    //    FindObjectOfType<Player>().transform.position = GameObject.FindGameObjectsWithTag("Train")[0].transform.Find("Player Start Point").position;
    //    //levelManager.Spawn(8);
    //    yield return new WaitForSeconds(2f);
    //    levelManager.loadingscreen.SetActive(false);
    //}
    protected void OnTriggerExit(Collider other) {
        if (other.GetComponent<Player>()) player.inputtext.SetActive(false);
    }
}
