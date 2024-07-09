using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "new npc Dialogue", menuName = "NPC Dialogue Line")]
public class DialogueSO : ScriptableObject
{
    public Data[] dialogueQuestions;
}
[System.Serializable]
public class Data
{
    public string[] assessDialogue;
    public string assessQuestion;
    public string[] assessOption;
    public bool[] assessAnswer;
    public string question;
    public string[] Dialogue;
    public string[] options;
    public bool[] answer;
    public string location;
    public Response[] response;// the  length of this is the length of the possible options
    public string outcome;
}

[System.Serializable]
public class Response
{
    public string[] response;
}