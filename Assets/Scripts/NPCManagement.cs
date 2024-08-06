using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManagement : MonoBehaviour
{
    public NPC myNPC;
    public GeneralQuestion myGeneral;
    LevelManager levelManager;
    NPCQuestion nPCQuestion;
    Player player;
    public int correctAmount=1;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        nPCQuestion = FindObjectOfType<NPCQuestion>();
        player = FindObjectOfType<Player>();
        myNPC = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (myNPC != null) {
            if (myNPC.tasksuccess == NPC.Task.Fail || (myNPC.tasksuccess == NPC.Task.Success)) {
                //myNPC.gameObject.SetActive(false);
                Destroy(myNPC.gameObject, .1f);
            }
        }
    }
    public void ContinueDialogue() {
        if (myNPC.dialogue.Length == 0) return;
        if (!myNPC.gameObject.activeSelf) return;
        if(myNPC.dialoguetext.text.Length < myNPC.dialogue[myNPC.currentline].Length) {
            myNPC.Stop();
            if (myNPC.currentline < myNPC.dialogue.Length - 1) { //check if there's more dialogue to type out
                myNPC.currentline += 1; // go to next line(just print out the whole line instead)
                myNPC.dialoguetext.text = ""; // reset text
                myNPC.dialogueco = StartCoroutine(myNPC.Dialogue());
            } else {
                myNPC.dialoguetext.text = myNPC.dialogue[myNPC.currentline];
                if (myNPC.indexDialogue == 2)
                {
                    myNPC.EndDialogue();
                    if (myNPC.hasdestination)
                    {
                        myNPC.followplayer = true;
                    }
                    else
                    {
                        levelManager.taskcompletescreen.SetActive(true);
                        myNPC.tasksuccess = NPC.Task.Success;
                        player.energy += player.maxenergy * .2f;
                        levelManager.upgradeText.SetActive(true);
                        //if (!upgraded) {
                        //    FindObjectOfType<Player>().maxenergy *= 1.5f;
                        //    upgraded = true;
                        //}
                    }
                    return;
                }
                else if (myNPC.indexDialogue == 3)
                {
                    myNPC.EndDialogue();
                    myNPC.tasksuccess = NPC.Task.Fail;
                    levelManager.taskcompletescreen.SetActive(true);
                    myNPC.questionbox.SetActive(true);
                    return;
                }
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
                if (myNPC.indexDialogue==2)
                {
                    myNPC.EndDialogue();
                    if (myNPC.hasdestination)
                    {
                        myNPC.followplayer = true;
                    }
                    else
                    {
                        levelManager.taskcompletescreen.SetActive(true);
                        myNPC.tasksuccess = NPC.Task.Success;
                        player.energy += player.maxenergy * .2f;
                        levelManager.upgradeText.SetActive(true);
                        //if (!upgraded) {
                        //    FindObjectOfType<Player>().maxenergy *= 1.5f;
                        //    upgraded = true;
                        //}
                    }
                    return;
                }else if(myNPC.indexDialogue == 3)
                {
                    myNPC.EndDialogue();
                    myNPC.tasksuccess = NPC.Task.Fail;
                    levelManager.taskcompletescreen.SetActive(true);
                    nPCQuestion.Explain();
                    return;
                }
                myNPC.questionbox.SetActive(true);//need to change this probably
                //myNPC.EndDialogue();
            }
        } 
    }
}
