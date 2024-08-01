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

    public DialogueQoute[] Dialogues;
    public string question;
    public DialOptions[] Dial;
    public LocationEnum locationE;
    public Response[] response;// the  length of this is the length of the possible options
    public string outcome;
    public OutcomeLocation outcomeLocation;
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
public enum OutcomeLocation
{
    taskCompletion,
    busStop,
    WheelchairZone,
    stationStaff,
    grandKid,
    emptySeat,
    busCaptain
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
