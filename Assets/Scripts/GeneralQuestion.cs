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
        if (other.GetComponent<Player>())
        {
            once = false;
            UpdateCanvas();
            Destroying();
            player.canMove = false;
            questionbox.SetActive(true);
            npcManager.myGeneral = this;
        }
    }
    override protected void Update()
    {
        if (myspawner == null) Destroy(gameObject);
        //else Destroy(gameObject, 20f);
        if (levelManager.level == LevelManager.Level.MRT || levelManager.level == LevelManager.Level.BusInterior) transform.localScale = trainsize; // change size in mrt level
        else transform.localScale = originalsize;
    }

    void Destroying()
    {
        for (int i = 0; i < this.transform.childCount + 1; i++)
        {
            if (i == this.transform.childCount)
            {
                this.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                MeshRenderer go = this.transform.GetChild(i).GetComponent<MeshRenderer>();
                go.enabled = false;
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
                if (sub == temp) typeIndexs.Add(i);

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
        if (dialogueData.dialogueQuestions[qnindex].answer[2] == true)
        {
            levelManager.optionCButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
        }
        if (dialogueData.dialogueQuestions[qnindex].answer[3] == true)
        {
            levelManager.optionDButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
        }
        questiontext.text = dialogueData.dialogueQuestions[qnindex].question;
        explaintext.text = dialogueData.dialogueQuestions[qnindex].explain;
        optionAtext.text = "a)" + dialogueData.dialogueQuestions[qnindex].options[0];
        optionBtext.text = "b)" + dialogueData.dialogueQuestions[qnindex].options[1];
        levelManager.optionCButton.SetActive(true);
        levelManager.optionDButton.SetActive(true);
        if (dialogueData.dialogueQuestions[qnindex].options.Length >= 3) optionCtext.text = "c)" + dialogueData.dialogueQuestions[qnindex].options[2];
        else
        {
            optionCtext.text = "";
            levelManager.optionCButton.SetActive(false);
        }
        if (dialogueData.dialogueQuestions[qnindex].options.Length >= 4) optionDtext.text = "d)" + dialogueData.dialogueQuestions[qnindex].options[3];
        else
        {
            optionDtext.text = "";
            levelManager.optionDButton.SetActive(false);
        }
    }
}
