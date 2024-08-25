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
    [TextArea(2, 10)]
    public string question;
    public bool isMutli;
    public TypeQuestion typing;
    [TextArea(2, 10)]
    public string[] options;    
    public bool[] answer;
    [TextArea(2, 10)]
    public string explain;
}