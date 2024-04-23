using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{
    public enum Options { CorrectOption, WrongOption, nulloption };
    public Options option;
    NPCManagement npcmanager;
    // Start is called before the first frame update
    void Start() {
        npcmanager = FindObjectOfType<NPCManagement>();
    }

    // Update is called once per frame
    void Update() {

    }
    public void AnswerQuestion() {
        if (option == Options.WrongOption) {
            Explain();
        } 
        else if (option == Options.CorrectOption) {
            npcmanager.myNPC.questionbox.SetActive(false);
            npcmanager.myNPC.EndDialogue();
            // play some sound.
        }
    }
    public void Explain() {
        for(int i = 0; i < npcmanager.myNPC.questionbox.transform.childCount; i++) {
            GameObject go = npcmanager.myNPC.questionbox.transform.GetChild(i).gameObject;
            if (go.name == "Explanation" || go.name == "Question Panel" || go.name == "Close Button") go.SetActive(true);
            else go.SetActive(false);
        }
    }
    public void CloseQuestion() {
        npcmanager.myNPC.questionbox.SetActive(false);
        npcmanager.myNPC.EndDialogue();
        for (int i = 0; i < npcmanager.myNPC.questionbox.transform.childCount; i++) {
            GameObject go = npcmanager.myNPC.questionbox.transform.GetChild(i).gameObject;
            if (go.name == "Explanation" || go.name == "Close Button") go.SetActive(false);
            else go.SetActive(true);
        }
    }
}
