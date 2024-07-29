using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class NPCQuestion : MonoBehaviour
{
    public enum Options { CorrectOption, WrongOption, nulloption };
    public Options option;
    NPCManagement npcmanager;
    LevelManager levelManager;
    Player player;
    public AudioClip correctsound, wrongsound;
    public int indexQuestion;
    // Start is called before the first frame update
    void Start() {
        npcmanager = FindObjectOfType<NPCManagement>();
        levelManager = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<Player>();
    }

    public void AnswerQuestion() {
        if (player.NPC == false)
        {
            if (option == Options.WrongOption)
            {
                levelManager.upgradeText.SetActive(false);
                Explain();
                levelManager.tasksuccesstext.text = "Wrong!";
                levelManager.taskcompletescreen.SetActive(true);
                levelManager.taskfailimg.SetActive(true);
                AudioManager.instance.PlaySFX(wrongsound);
            }
            else if (option == Options.CorrectOption)
            {
                Explain();
                levelManager.taskcompletescreen.SetActive(true);
                levelManager.upgradeText.SetActive(true);
                levelManager.boost.GetComponentInChildren<TMP_Text>().text = "Timed Boost: Invincibility";
                levelManager.boost.SetActive(true);
                foreach(var i in levelManager.boost.GetComponentsInChildren<Image>())i.enabled = false;
                player.Invincibility();
                //if (player.originalInvincibleTime < player.maxInvincibleTime)player.originalInvincibleTime += 10;
                levelManager.taskfailimg.SetActive(false);
                levelManager.tasksuccesstext.text = "Correct!";
                //levelManager.questionbox.SetActive(false);
                AudioManager.instance.PlaySFX(correctsound);
               
                
            }
            return;
        }


        if (option == Options.WrongOption) {
            //Explain();
            levelManager.upgradeText.SetActive(false);
            npcmanager.myNPC.questionbox.SetActive(false);
            if(npcmanager.myNPC.indexDialogue == 1)
            {
                
                npcmanager.myNPC.indexDialogue += 2;
                AudioManager.instance.PlaySFX(wrongsound);
                npcmanager.myNPC.Response(indexQuestion);
                npcmanager.myNPC.dialoguebox.SetActive(true);
                levelManager.dialoguescreen.SetActive(true);
                npcmanager.myNPC.StartDialogue();
                return;
            }
            // NPC responds to player's choice here
            npcmanager.myNPC.EndDialogue();
            levelManager.taskcompletescreen.SetActive(true);
            npcmanager.myNPC.tasksuccess = NPC.Task.Fail;
            AudioManager.instance.PlaySFX(wrongsound);
        } 
        else if (option == Options.CorrectOption) {
           
            //Explain();
            npcmanager.myNPC.questionbox.SetActive(false);
            if (npcmanager.myNPC.indexDialogue < 1)
            {
                npcmanager.myNPC.indexDialogue += 1;
                npcmanager.myNPC.UpdateCanvas();
                levelManager.dialoguescreen.SetActive(true);
                npcmanager.myNPC.StartDialogue();
                return;
            }
            npcmanager.myNPC.indexDialogue += 1;
            npcmanager.myNPC.Response(indexQuestion);
            npcmanager.myNPC.dialoguebox.SetActive(true);
            levelManager.dialoguescreen.SetActive(true);
            npcmanager.myNPC.StartDialogue();
            AudioManager.instance.PlaySFX(correctsound);/*
            if (npcmanager.myNPC.hasdestination) {
                npcmanager.myNPC.followplayer = true;
            } 
            else {
                levelManager.taskcompletescreen.SetActive(true);
                npcmanager.myNPC.tasksuccess = NPC.Task.Success;
                player.energy += player.maxenergy * .2f;
                levelManager.upgradeText.SetActive(true);
                //if (!upgraded) {
                //    FindObjectOfType<Player>().maxenergy *= 1.5f;
                //    upgraded = true;
                //}
            }*/
            
        }


    }
    public void Explain() {
               
        Time.timeScale = 0;
        levelManager.upgradeText.SetActive(false);
        if (npcmanager.myNPC!=null)
        {
            for(int i = 0; i < npcmanager.myNPC.questionbox.transform.childCount; i++) {
                GameObject go = npcmanager.myNPC.questionbox.transform.GetChild(i).gameObject;
                if (go.name == "Explanation" || go.name == "Question Panel" || go.name == "Close Button") 
                {
                    print(go.name);
                    go.SetActive(true);
                }
                else go.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < npcmanager.myGeneral.questionbox.transform.childCount; i++)
            {
                GameObject go = npcmanager.myGeneral.questionbox.transform.GetChild(i).gameObject;
                if (go.name == "Explanation" || go.name == "Question Panel" || go.name == "Close Button") go.SetActive(true);
                else go.SetActive(false);
            }


        }

    }
    public void CloseQuestion() {
        player.canMove = true;
        Time.timeScale = 1;
        if (npcmanager.myNPC == true)
        {
            npcmanager.myNPC.questionbox.SetActive(false);
            npcmanager.myNPC.EndDialogue();
            for (int i = 0; i < npcmanager.myNPC.questionbox.transform.childCount; i++)
            {
                GameObject go = npcmanager.myNPC.questionbox.transform.GetChild(i).gameObject;
                if (go.name == "Explanation" || go.name == "Close Button") go.SetActive(false);
                else go.SetActive(true);
            }
        }
        else
        {
            npcmanager.myGeneral.questionbox.SetActive(false);
            for (int i = 0; i < npcmanager.myGeneral.questionbox.transform.childCount; i++)
            {
                GameObject go = npcmanager.myGeneral.questionbox.transform.GetChild(i).gameObject;
                if (go.name == "Explanation" || go.name == "Close Button") go.SetActive(false);
                else go.SetActive(true);
            }
            npcmanager.myGeneral = null;
        }
    }

}
