using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject inputtext;
    NPCManagement npcmanager;
    LevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        inputtext.SetActive(false);
        npcmanager = FindObjectOfType<NPCManagement>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) /*&& FindObjectOfType<Bus>() && FindObjectOfType<Bus>().transitioned*/ && !gameObject.CompareTag("Finish")) {
            //transform.SetParent(null);
            foreach (var i in FindObjectsOfType<Tile>()) {
                if (!i.gameObject.CompareTag("Train")) {
                    Destroy(i.gameObject);
                }
            }//levelManager.mrt.SetActive(true);
            //levelManager.Spawn(1);
            FindObjectOfType<Bus>().transitioned = false;
            StartCoroutine(levelManager.MoveToTrain());
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (gameObject.CompareTag("Finish")) {
            if (npcmanager.myNPC == null) return;
            if (other.GetComponent<Player>() && npcmanager.myNPC.followplayer) inputtext.SetActive(true);
        } 
        else {
            if (other.GetComponent<Player>()) {
                inputtext.SetActive(true);
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
    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<Player>()) inputtext.SetActive(false);
    }
}
