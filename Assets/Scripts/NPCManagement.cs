using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManagement : MonoBehaviour
{
    public NPC myNPC;
    public GeneralQuestion myGeneral;
    LevelManager levelManager;
    Player player;
    public int correctAmount=1;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<Player>();
        myNPC = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (myNPC != null) {
            if (myNPC.tasksuccess == NPC.Task.Fail) {
                //myNPC.gameObject.SetActive(false);
                Destroy(myNPC.gameObject, .1f);
            }
            else if(myNPC.tasksuccess == NPC.Task.Success) {
                myNPC.gameObject.SetActive(false);
                //foreach(var i in myNPC.GetComponents<Renderer>())i.enabled = false;
                //foreach(var i in myNPC.GetComponents<Collider>())i.isTrigger = true;
                //myNPC = null;
            }
        }
    }
    public void ContinueDialogue() {
        if (myNPC.dialogue.Length == 0) return;
        if (!myNPC.gameObject.activeSelf) return;
        if(myNPC.dialoguetext.text.Length < myNPC.dialogue[myNPC.currentline].Length) { // dialogue hasnt finished typing
            myNPC.Stop();
            myNPC.dialoguetext.text = myNPC.dialogue[myNPC.currentline];
            /*
            if (myNPC.currentline < myNPC.dialogue.Length - 1) { //check if there's more dialogue to type out
                myNPC.dialoguetext.text = myNPC.dialogue[myNPC.currentline];
                //myNPC.currentline += 1; // go to next line(just print out the whole line instead)
                //myNPC.dialoguetext.text = ""; // reset text
                //myNPC.dialogueco = StartCoroutine(myNPC.Dialogue());
            } 
            else {
                myNPC.dialoguetext.text = myNPC.dialogue[myNPC.currentline];
                if (myNPC.indexDialogue == 2)//this is npc second correct outcome 
                {
                    myNPC.EndDialogue();
                    if (myNPC.hasdestination)
                    {
                        Time.timeScale = 1;
                        myNPC.followplayer = true;
                        myNPC.Guide();
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
                else if (myNPC.indexDialogue == 3)//this is npc second wrong outcome 
                {
                    myNPC.EndDialogue();
                    myNPC.tasksuccess = NPC.Task.Fail;
                    levelManager.taskcompletescreen.SetActive(true);
                    return;
                }
                if (!myNPC.questionbox.activeSelf) myNPC.questionbox.SetActive(true);
                //myNPC.EndDialogue();
            }*/
        }
        else if (myNPC.dialoguetext.text.Length >= myNPC.dialogue[myNPC.currentline].Length) { // check if all the text in the current line has been typed out.
            if (myNPC.currentline < myNPC.dialogue.Length - 1) { //check if there's more dialogue to type out
                myNPC.currentline += 1; // go to next line
                myNPC.dialoguetext.text = ""; // reset text
                myNPC.dialogueco = StartCoroutine(myNPC.Dialogue());
            } 
            else {
                //myNPC.dialoguetext.text = myNPC.dialogue[myNPC.currentline];
                if (myNPC.indexDialogue==2)
                {
                    myNPC.EndDialogue();
                    if (myNPC.hasdestination)
                    {
                        Time.timeScale = 1;
                        myNPC.followplayer = true;
                        myNPC.Guide();
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
                else if(myNPC.indexDialogue == 3)
                {
                    myNPC.EndDialogue();
                    myNPC.tasksuccess = NPC.Task.Fail;
                    levelManager.taskcompletescreen.SetActive(true);
                    return;
                }
                myNPC.questionbox.SetActive(true);//need to change this probably
                //myNPC.EndDialogue();
            }
        } 
    }
}
