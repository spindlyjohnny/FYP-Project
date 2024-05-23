using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Interactable : MonoBehaviour
{
    public Player player;
    protected NPCManagement npcmanager;
    LevelManager levelManager;
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
        if (Input.GetKeyDown(KeyCode.F) && !gameObject.CompareTag("Finish") && !GetComponent<TrainObstacle>()) {
            //transform.SetParent(null);
            
            //levelManager.Spawn(1);
            print("yes");
            player.inputtext.SetActive(false);
            FindObjectOfType<Bus>().transitioned = false;
            StartCoroutine(levelManager.MoveToTrain());
            
        }
    }
    protected void OnTriggerEnter(Collider other) {
        if (gameObject.CompareTag("Finish")) {
            if (npcmanager.myNPC == null) return;
            if (other.GetComponent<Player>() && npcmanager.myNPC.followplayer) player.inputtext.SetActive(true);
        } 
        else {
            if (other.GetComponent<Player>()) {
                player.inputtext.SetActive(true);
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
