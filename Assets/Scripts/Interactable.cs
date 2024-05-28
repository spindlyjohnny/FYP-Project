using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Interactable : MonoBehaviour
{
    public Player player;
    protected NPCManagement npcmanager;
    LevelManager levelManager;
    public string location;
    bool target = false;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        //inputtext.SetActive(false);
        npcmanager = FindObjectOfType<NPCManagement>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        var detector = Physics.OverlapSphere(transform.position, .5f); // detects NPC
        for (int i = 0; i < detector.Length; i++) {
            if (!detector[i]) return;
            if (detector[i].gameObject == npcmanager.myNPC.gameObject) target = true;
            if (GetComponentInParent<RoadTile>()) { // sets NPC street to gameobject if it's a roadtile
                npcmanager.myNPC.street = GetComponentInParent<RoadTile>();
            }
            //else if(detector[i]) // mrt tile.
        }
        // if npc is touching self
        if (target && Input.GetKeyDown(KeyCode.F) && npcmanager.myNPC && npcmanager.myNPC.npcLocation == location/*!gameObject.CompareTag("Finish") && !GetComponent<TrainObstacle>()*/) {
            //transform.SetParent(null);
            npcmanager.myNPC.Transitioninator();
            //levelManager.Spawn(1);
            if (gameObject.CompareTag("Train")) {
                print("yes");
                player.inputtext.SetActive(false);
                FindObjectOfType<Bus>().transitioned = false;
                levelManager.MoveToTrain();
            }

        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, .5f);
    }
    protected void OnTriggerEnter(Collider other) {
        if (target) {
            //if (npcmanager.myNPC == null) return;
            if (other.GetComponent<Player>() && npcmanager.myNPC.followplayer) player.inputtext.SetActive(true);
        } 
        else {
            if (other.GetComponent<Player>()) {
                player.inputtext.SetActive(false);
            }
        }
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
