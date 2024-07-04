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
    public string type;
    public string[] options;
    public bool[] answer;
    public string explain;
}