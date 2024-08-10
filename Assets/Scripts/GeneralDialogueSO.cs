using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "new general Dialogue", menuName = "General Dialogue Line")]
public class GeneralDIalogueSO : ScriptableObject
{
    public GeneralData[] dialogueQuestions;
}
[System.Serializable]
public class GeneralData
{
    public string question;
    public bool isMutli;
    public string type;
    public TypeQuestion typing;
    public string[] options;    
    public bool[] answer;
    public string explain;
}