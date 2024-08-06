using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeneralQuestion : Collectible
{
    [Header("Dialogue")]
    //[HideInInspector]
    public GameObject questionbox;
    TMP_Text  questiontext, explaintext, optionAtext, optionBtext, optionCtext, optionDtext;
    public string[] type;
    public GeneralDIalogueSO dialogueData;
    NPCManagement npcManager;
    Player player;
    public Coroutine dialogueco;
    [SerializeField] AudioClip dialoguesound, correctsound;
    public enum Task { Success, Fail, Default }
    public Task tasksuccess;
    bool once=true;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        originalsize = transform.localScale;
        levelManager = FindObjectOfType<LevelManager>();
        npcManager = FindObjectOfType<NPCManagement>();
        player = FindObjectOfType<Player>();
        SettingString();
        tasksuccess = Task.Default;
        switch (LevelManager.levelNum)
        {
            case LevelManager.LevelNum.Level1:
                type = new string[2];
                type[0]="Elderly";
                type[1]="Wheelchair";
                break;
            case LevelManager.LevelNum.Level2:
                type = new string[2];
                type[0] = "Visual";
                type[1] = "Hearing";
                break;
            case LevelManager.LevelNum.Level3:
                type = new string[3];
                type[0] = "Autism";
                type[1] = "Invisible";
                type[2] = "Intellectual";
                break;
        }
    }

    void SettingString()
    {
        //setting the refeqrence canvas reference
        questiontext = levelManager.questiontext;
        explaintext = levelManager.explaintext;
        optionAtext = levelManager.optionAtext;
        optionBtext = levelManager.optionBtext;
        optionCtext = levelManager.optionCtext;
        optionDtext = levelManager.optionDtext;
        questionbox = levelManager.questionbox;
        //turning the visibility of option c and D on
        levelManager.optionCButton.SetActive(true);
        levelManager.optionDButton.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (once == false) return;
        if (other.GetComponent<Player>() && !player.NPC)
        {
            once = false;
            UpdateCanvas();
            Destroying();
            player.canMove = false;
            //player.NPC = true;
            questionbox.SetActive(true);
            npcManager.myGeneral = this;
            Time.timeScale = 0;
        }
    }
    override protected void Update()
    {
        if (myspawner == null) Destroy(gameObject);
        //else Destroy(gameObject, 20f);
        if (levelManager.level == LevelManager.Level.MRT) {
            transform.localScale = trainsize; // change size in mrt level
        } 
        else if (levelManager.level == LevelManager.Level.BusInterior) {
            transform.localScale = bussize;
        } 
        else {
            transform.localScale = originalsize;
        }
    }

    void Destroying()
    {
        for (int i = 0; i < transform.childCount + 1; i++)
        {
            if (i == transform.childCount)
            {
                GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                GameObject go = transform.GetChild(i).gameObject;
                go.SetActive(false);
            }


        }
    }

    void UpdateCanvas()//this function is to be called when the player collide with the questionaire
    {
        List<int> typeIndexs = new List<int>(0);
        for(int e=0; e < dialogueData.dialogueQuestions.Length; e++)
        {
            for (int i = 0; i < type.Length; i++)
            {
                string sub = type[i].Substring(0, 3);
                string temp = dialogueData.dialogueQuestions[e].type.Substring(0, 3);
                if (sub == temp)
                {
                    typeIndexs.Add(e);
                    print(temp);
                }

            }
        }
        int index = Random.Range(0, typeIndexs.Count);
        print(typeIndexs.Count);
        int qnindex = typeIndexs[index];//random index for a question that is related to the current type
        levelManager.optionAButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        levelManager.optionBButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        levelManager.optionCButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        levelManager.optionDButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        if (dialogueData.dialogueQuestions[qnindex].answer[0] == true)
        {
            levelManager.optionAButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
        }
        if (dialogueData.dialogueQuestions[qnindex].answer[1] == true)
        {
            levelManager.optionBButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
        }
        if (dialogueData.dialogueQuestions[qnindex].answer.Length >= 3 && dialogueData.dialogueQuestions[qnindex].answer[2] == true)
        {
            levelManager.optionCButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
        }
        if (dialogueData.dialogueQuestions[qnindex].answer.Length == 4 && dialogueData.dialogueQuestions[qnindex].answer[3] == true)
        {
            levelManager.optionDButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
        }
        questiontext.text = dialogueData.dialogueQuestions[qnindex].question;
        explaintext.text = dialogueData.dialogueQuestions[qnindex].explain;
        optionAtext.text = "A." + dialogueData.dialogueQuestions[qnindex].options[0];
        optionBtext.text = "B." + dialogueData.dialogueQuestions[qnindex].options[1];
        levelManager.optionCButton.SetActive(true);
        levelManager.optionDButton.SetActive(true);
        if (dialogueData.dialogueQuestions[qnindex].options.Length >= 3) optionCtext.text = "C." + dialogueData.dialogueQuestions[qnindex].options[2];
        else
        {
            optionCtext.text = "";
            levelManager.optionCButton.SetActive(false);
        }
        if (dialogueData.dialogueQuestions[qnindex].options.Length >= 4) optionDtext.text = "D." + dialogueData.dialogueQuestions[qnindex].options[3];
        else
        {
            optionDtext.text = "";
            levelManager.optionDButton.SetActive(false);
        }
        int correctAmount = 1;
        if (dialogueData.dialogueQuestions[qnindex].isMutli == true) 
        {
            correctAmount = 0;
            foreach (bool answer in dialogueData.dialogueQuestions[qnindex].answer)
            {
                if (answer == true)
                {
                    correctAmount += 1;
                }
            }
        }
        npcManager.correctAmount = correctAmount;


    }
}
