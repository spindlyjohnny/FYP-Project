using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeneralQuestion : Collectible
{
    [Header("Dialogue")]
    //[HideInInspector]
    public GameObject questionbox;
    public string  optionA, optionB, optionC, optionD;
    TMP_Text  questiontext, explaintext, optionAtext, optionBtext, optionCtext, optionDtext;
    public string[] questions, explains,options,type,currentTypes;
    public List<string> filteredOptions = new List<string>(0);
    public List<OptionsOfQuestions> optionList;
    public TextAsset optionsFile,Question,ExplainationFile,TypeFile;
    NPCManagement npcManager;
    NPC npc;
    Player player;
    public Coroutine dialogueco;
    [SerializeField] AudioClip dialoguesound, correctsound;
    public enum Task { Success, Fail, Default }
    public Task tasksuccess;
    public Answer[] answer = new Answer[51];
    public List<string> newstr = new List<string>(0);
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
        type = TypeFile.text.Split("\n");
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
        for(int e=0; e < currentTypes.Length; e++)
        {
            for (int i = 0; i < type.Length; i++)
            {
                string sub = type[i].Substring(0, 3);
                string temp = currentTypes[e].Substring(0, 3);
                if (sub == temp) typeIndexs.Add(i);

            }
        }
        int index = Random.Range(0, typeIndexs.Count);
        int qnindex = typeIndexs[index];//random index for a question that is related to the current type
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
        int ind = 0;
        for(int i = 0; i < optionList[qnindex].option.Count; i++)
        {            
            for(int c = 0; c < optionList[qnindex].option[i].Length; c++)
            {
                if(optionList[qnindex].option[i].ToCharArray()[c]== options[0].ToCharArray()[0])
                {
                    print(optionList[qnindex].option[i].ToCharArray().Length - c);
                    string temp = optionList[qnindex].option[i].Substring(0, c);
                    string temproray = optionList[qnindex].option[i].Substring(c+1, optionList[qnindex].option[i].ToCharArray().Length-c-1 );
                    newstr.Add(temp + temproray);
                    print(ind);
                    optionList[qnindex].option[i] = newstr[ind];
                    ind+=1;
                }
            }
        }
        explaintext.text = explains[qnindex];
        optionA = optionList[qnindex].option[0];
        optionB = optionList[qnindex].option[1];
        if (optionList[qnindex].option.Count >= 3) optionC = this.optionList[qnindex].option[2];
        if (optionList[qnindex].option.Count >= 4) optionD = this.optionList[qnindex].option[3];
        questiontext.text = questions[qnindex];
        explaintext.text = explains[qnindex];
        optionAtext.text = "a)" + optionA;
        optionBtext.text = "b)" + optionB;
        levelManager.optionCButton.SetActive(true);
        levelManager.optionDButton.SetActive(true);
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
