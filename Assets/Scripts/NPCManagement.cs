using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManagement : MonoBehaviour
{
    public NPC myNPC;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ContinueDialogue() {
        if (myNPC.dialogue.Length == 0) return;
        if (!myNPC.gameObject.activeSelf) return;
        if(myNPC.dialoguetext.text.Length < myNPC.dialogue[myNPC.currentline].Length) {
            StopCoroutine(myNPC.dialogueco);
            if (myNPC.currentline < myNPC.dialogue.Length - 1) { //check if there's more dialogue to type out
                myNPC.currentline += 1; // go to next line
                myNPC.dialoguetext.text = ""; // reset text
                myNPC.dialogueco = StartCoroutine(myNPC.Dialogue());
            } else {
                myNPC.dialoguetext.text = myNPC.dialogue[myNPC.currentline];
                if (!myNPC.questionbox.activeSelf) myNPC.questionbox.SetActive(true);
                //myNPC.EndDialogue();
            }
        }
        else if (myNPC.dialoguetext.text.Length >= myNPC.dialogue[myNPC.currentline].Length) { // check if all the text in the current line has been typed out.
            if (myNPC.currentline < myNPC.dialogue.Length - 1) { //check if there's more dialogue to type out
                myNPC.currentline += 1; // go to next line
                myNPC.dialoguetext.text = ""; // reset text
                myNPC.dialogueco = StartCoroutine(myNPC.Dialogue());
            } 
            else {
                if (!myNPC.questionbox.activeSelf) myNPC.questionbox.SetActive(true);
                //myNPC.EndDialogue();
            }
        } 
    }
}
