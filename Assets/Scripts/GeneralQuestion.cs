using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeneralQuestion : Collectible
{
    [Header("Dialogue")]
    GameObject questionbox;
    public string  optionA, optionB, optionC, optionD;
    TMP_Text  questiontext, explaintext, optionAtext, optionBtext, optionCtext, optionDtext;
    public string[] questions, explains,options;
    public List<string> filteredOptions = new List<string>(0);
    public List<OptionsOfQuestions> optionList;
    public TextAsset optionsFile,Question,ExplainationFile;
    LevelManager levelManager;
    NPC npc;
    Player player;
    public Coroutine dialogueco;
    [SerializeField] AudioClip dialoguesound, correctsound;
    public enum Task { Success, Fail, Default }
    public Task tasksuccess;
    public Answer[] answer = new Answer[51];
    // Start is called before the first frame update
    protected virtual void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<Player>();
        SettingString();
        tasksuccess = Task.Default;
        bool continueQuestion = false;
        int optionIndex = 0;
        char quotationMark = options[0].ToCharArray()[0];
        for (int i = 0; i < filteredOptions.Count; i++)
        {
            int numberOfCharacter = filteredOptions[i].ToCharArray().Length;

            if (continueQuestion == false && filteredOptions[i].ToCharArray()[0] == quotationMark)
            {
                continueQuestion = true;
                this.optionList[optionIndex].option.Add(filteredOptions[i]);
            }
            else if (continueQuestion == true && filteredOptions[i].ToCharArray()[numberOfCharacter - 2] != quotationMark)
            {
                this.optionList[optionIndex].option.Add(filteredOptions[i]);
            }
            else if (continueQuestion == true && filteredOptions[i].ToCharArray()[numberOfCharacter - 2] == quotationMark)
            {
                this.optionList[optionIndex].option.Add(filteredOptions[i]);
                continueQuestion = false;
                optionIndex += 1;
            }
        }
    }

    void SettingString()
    {
        print("yes");
        //setting the reference canvas reference
        questiontext = levelManager.questiontext;
        explaintext = levelManager.explaintext;
        optionAtext = levelManager.optionAtext;
        optionBtext = levelManager.optionBtext;
        optionCtext = levelManager.optionCtext;
        optionDtext = levelManager.optionDtext;
        questionbox = levelManager.questionbox;
        //setting the right
        options = optionsFile.text.Split("\n");
        explains = ExplainationFile.text.Split("\n");
        questions = Question.text.Split("\n");
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i] != options[1])
            {
                filteredOptions.Add(options[i]);
            }
        }
        //turning the visibility of option c and D on
        levelManager.optionCButton.SetActive(true);
        levelManager.optionDButton.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            UpdateCanvas();
            player.canMove = false;
            questionbox.SetActive(true);
        }
    }
    override protected void Update()
    {
        
    }

    void UpdateCanvas()//this function is to be called when the player collide with the questionaire
    {
        npc = FindObjectOfType<NPC>();
        answer = npc.answer;
        int qnindex = Random.Range(0, questions.Length);//random index for a question
        levelManager.optionAButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        levelManager.optionBButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        levelManager.optionCButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        levelManager.optionDButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        foreach (int i in answer[qnindex].element)
        {
            if (i == 1)
            {
                levelManager.optionAButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
            }
            else if (i == 2)
            {
                levelManager.optionBButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
            }
            else if (i == 3)
            {
                levelManager.optionCButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
            }
            else if (i == 4)
            {
                levelManager.optionDButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
            }
        }
        optionA = optionList[qnindex].option[0];
        optionB = optionList[qnindex].option[1];
        if (optionList[qnindex].option.Count >= 3) optionC = this.optionList[qnindex].option[2];
        if (optionList[qnindex].option.Count >= 4) optionD = this.optionList[qnindex].option[3];
        questiontext.text = questions[qnindex];
        explaintext.text = explains[qnindex];
        optionAtext.text = "a)" + optionA;
        optionBtext.text = "b)" + optionB;
        if (optionList[qnindex].option.Count >= 3) optionCtext.text = "c)" + optionC;
        else
        {
            optionCtext.text = "";
            levelManager.optionCButton.SetActive(false);
        }
        if (optionList[qnindex].option.Count >= 4) optionDtext.text = "d)" + optionD;
        else
        {
            optionDtext.text = "";
            levelManager.optionDButton.SetActive(false);
        }
    }
}
