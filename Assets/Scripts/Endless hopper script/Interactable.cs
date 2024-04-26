using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject inputtext;
    NPCManagement npcmanager;
    // Start is called before the first frame update
    void Start()
    {
        inputtext.SetActive(false);
        npcmanager = FindObjectOfType<NPCManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        if (npcmanager.myNPC == null) return;
        if (other.GetComponent<Player>() && npcmanager.myNPC.followplayer) inputtext.SetActive(true);
    }
    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<Player>()) inputtext.SetActive(false);
    }
}
