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
    public string question;
    public string[] Dialogue;    
    public string[] options;
    public bool[] answer;
    public string location;
    public string outcome;
}
