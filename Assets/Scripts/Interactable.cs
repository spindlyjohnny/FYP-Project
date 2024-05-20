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
    private void OnTriggerStay(Collider other) {
        if (other.GetComponent<Player>()) {
            if(Input.GetKeyDown(KeyCode.F) && FindObjectOfType<Bus>() && FindObjectOfType<Bus>().transitioned && !gameObject.CompareTag("Finish")) {
                //transform.SetParent(null);
                FindObjectOfType<Bus>().transitioned = false;
                FindObjectOfType<Player>().transform.position = GameObject.FindGameObjectWithTag("Train").transform.Find("Player Start Point").position;
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<Player>()) inputtext.SetActive(false);
    }
}
