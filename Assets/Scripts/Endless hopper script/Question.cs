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
        option = Options.nulloption;
    }

    // Update is called once per frame
    void Update() {

    }
    public void AnswerQuestion() {
        if (option == Options.WrongOption) {
           StartCoroutine(Explain());
        } else if (option == Options.CorrectOption) {
            npcmanager.myNPC.questionbox.SetActive(false);
            npcmanager.myNPC.EndDialogue();
        }
    }
    public IEnumerator Explain() {
        foreach(GameObject i in npcmanager.myNPC.questionbox.transform.GetComponentInChildren<Transform>()) {
            if (i.name == "Explanation" || i.name == "Question Panel") i.SetActive(true);
            else i.SetActive(false);
        }
        yield return new WaitForSeconds(10);
        npcmanager.myNPC.EndDialogue();
    }
}
