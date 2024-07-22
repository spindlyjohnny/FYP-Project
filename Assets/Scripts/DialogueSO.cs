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
    public string assessQuestion;
    public DialogueQoute[] assDialogue;
    public DialOptions[] assDial;
    public string[] assessDialogue;    
    public string[] assessOption = new string[2];
    public bool[] assessAnswer;

    public DialogueQoute[] Dialogues;
    public DialOptions[] Dial;
    public string question;
    public string[] Dialogue;
    public string[] options;
    public bool[] answer;
    public LocationEnum locationE;
    public Response[] response;// the  length of this is the length of the possible options
    public string outcome;
}

[System.Serializable]
public class Response
{
    public string[] response;
}

[System.Serializable]
public enum LocationEnum
{
    Pavement,
    BusInterior,
    Mrt,
    BusStop,
    Default,
    WheelchairZone,
    Null
}

[System.Serializable]
public class DialogueQoute
{
    public string speechLine;
    public bool npcTalking;
}

[System.Serializable]
public class DialOptions
{
    public string option;
    public bool isCorrect;
}
