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
    bool upgraded;
    public AudioClip correctsound, wrongsound;
    // Start is called before the first frame update
    void Start() {
        npcmanager = FindObjectOfType<NPCManagement>();
        levelManager = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update() {

    }
    public void AnswerQuestion() {
        if (npcmanager.myNPC == null)
        {
            if (option == Options.WrongOption)
            {
                levelManager.upgradeText.SetActive(false);
                Explain();
                levelManager.tasksuccesstext.text = "Task failed!";
                levelManager.taskcompletescreen.SetActive(true);
                AudioManager.instance.PlaySFX(wrongsound);
            }
            else if (option == Options.CorrectOption)
            {
                Explain();
                levelManager.taskcompletescreen.SetActive(true);             
                if (!upgraded)
                {
                    levelManager.upgradeText.SetActive(true);
                    levelManager.boost.GetComponentInChildren<TMP_Text>().text = "Invincibility (10s)";
                    levelManager.boost.GetComponentInChildren<Image>().enabled = false;
                    player.Invincible();
                    upgraded = true;
                }
                else
                {
                    levelManager.upgradeText.SetActive(false);
                }

                levelManager.tasksuccesstext.text = "Task success!";
                //levelManager.questionbox.SetActive(false);
                AudioManager.instance.PlaySFX(correctsound);
               
                
            }
            player.canMove = true;
            return;
        }


        if (option == Options.WrongOption) {
            //Explain();
            levelManager.upgradeText.SetActive(false);
            npcmanager.myNPC.questionbox.SetActive(false);
            npcmanager.myNPC.EndDialogue();
            levelManager.taskcompletescreen.SetActive(true);
            npcmanager.myNPC.tasksuccess = NPC.Task.Fail;
            AudioManager.instance.PlaySFX(wrongsound);
        } 
        else if (option == Options.CorrectOption) {
            //Explain();
            npcmanager.myNPC.questionbox.SetActive(false);
            npcmanager.myNPC.EndDialogue();
            if (npcmanager.myNPC.hasdestination) {
                npcmanager.myNPC.followplayer = true;
            } 
            else {
                levelManager.taskcompletescreen.SetActive(true);
                npcmanager.myNPC.tasksuccess = NPC.Task.Success;
                //if (!upgraded) {
                //    FindObjectOfType<Player>().maxenergy *= 1.5f;
                //    upgraded = true;
                //}
            }
            AudioManager.instance.PlaySFX(correctsound);
        }

    }
    public void Explain() {
        levelManager.upgradeText.SetActive(false);
        if (npcmanager.myNPC!=null)
        {
            for(int i = 0; i < npcmanager.myNPC.questionbox.transform.childCount; i++) {
                GameObject go = npcmanager.myNPC.questionbox.transform.GetChild(i).gameObject;
                if (go.name == "Explanation" || go.name == "Question Panel" || go.name == "Close Button") go.SetActive(true);
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

        }
    }
}
