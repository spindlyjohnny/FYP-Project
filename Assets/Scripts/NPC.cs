using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
public class NPC : MonoBehaviour
{
    public GameObject dialoguebox,questionbox;
    CameraController cam;
    Player player;
    public string[] dialogue = new string[0];
    public List<string> dialogueList;
    public string optionA,optionB,optionC,optionD,npcLocation;
    public TMP_Text dialoguetext,questiontext,explaintext,optionAtext,optionBtext,optionCtext,optionDtext;
    public TMP_Text nametext;
    public float wordspeed;
    public int currentline;
    //bool spoken;
    public bool followplayer;
    public NPCManagement npcmanager;
    string[] names, questions, explains,outcomes;
    string[] options,questionLocation;
    public List<string> filteredOptions= new List<string>(0);
    public List<OptionsOfQuestions> optionList;
    public TextAsset optionsFile, dialogueFile, questionFile,locationFile, nameFile, explainsFile,outcomesFile;
    public float movespeed;
    LevelManager levelManager;
    public Vector3 startpos;
    public enum Task { Success,Fail,Default}
    public Task tasksuccess;
    public bool hasdestination; // set in inspector if NPC has a destination.
    public Coroutine dialogueco;
    [SerializeField]AudioClip dialoguesound, correctsound;
    public RoadTile street;
    public Answer[] answer = new Answer[51];
    public string sub;
    public string temp;
    public int character = 2;
    public Image avatar;
    public Sprite dialogueSprite;
    // Start is called before the first frame update
    void Start()
    {
        //setting the references
        npcmanager = FindObjectOfType<NPCManagement>();
        levelManager = FindObjectOfType<LevelManager>();
        cam = FindObjectOfType<CameraController>();
        player = FindObjectOfType<Player>();
        names = nameFile.text.Split("\n");
        questions = questionFile.text.Split("\n");
        explains = explainsFile.text.Split("\n");
        questionLocation= locationFile.text.Split("\n");
        outcomes= outcomesFile.text.Split("\n");
        string[] dialogueEx = new string[0];
        dialogueEx = dialogueFile.text.Split("\n");
        tasksuccess = Task.Default;
        options = optionsFile.text.Split("\n");

        //setting the text reference to the right canvas
        nametext = levelManager.nametext;
        dialoguetext = levelManager.dialoguetext;
        questiontext = levelManager.questiontext;
        explaintext = levelManager.explaintext;
        optionAtext = levelManager.optionAtext;
        optionBtext = levelManager.optionBtext;
        optionCtext = levelManager.optionCtext;
        optionDtext = levelManager.optionDtext;
        dialoguebox = levelManager.dialoguebox;
        questionbox = levelManager.questionbox;
        avatar = levelManager.npcAvatar;
        //avatar.sprite = dialogueSprite;
        //turning the visibility on
        levelManager.optionCButton.SetActive(true);
        levelManager.optionDButton.SetActive(true);

        for (int i = 0; i < options.Length; i++)
        {
             filteredOptions.Add(options[i]);
        }

        for(int i = 0; i < dialogueEx.Length; i++)
        {
            dialogueList.Add(dialogueEx[i]);
        }
       
        bool continuing = false;
        int optionIndex = 0;
        char quotationMark = filteredOptions[0].ToCharArray()[0];
        for (int i = 0; i < dialogueList.Count; i++)
        {
            int numberOfCharacter = dialogueList[i].ToCharArray().Length;
            if (continuing==false && dialogueList[i].ToCharArray()[0]== quotationMark && dialogueList[i].ToCharArray()[numberOfCharacter - character] != quotationMark)
            {
                continuing = true;
                this.optionList[optionIndex].Dialogue.Add(dialogueList[i]);
            }
            else if(continuing ==true && dialogueList[i].ToCharArray()[numberOfCharacter- character] != quotationMark)//end with no quotation
            {
                this.optionList[optionIndex].Dialogue.Add(dialogueList[i]);
            }
            else if(continuing == true && dialogueList[i].ToCharArray()[numberOfCharacter - character] == quotationMark)
            {
                
                this.optionList[optionIndex].Dialogue.Add(dialogueList[i]);
                continuing = false;
                optionIndex += 1;
            }
            else if(continuing == false && dialogueList[i].ToCharArray()[0] == quotationMark && dialogueList[i].ToCharArray()[numberOfCharacter - character] == quotationMark)
            {
                this.optionList[optionIndex].Dialogue.Add(dialogueList[i]);
                optionIndex += 1;
            }
        }
        continuing = false;
        optionIndex = 0;
        for (int i = 0; i < filteredOptions.Count; i++)
        {
            int numberOfCharacter = filteredOptions[i].ToCharArray().Length;
            if (continuing==false && filteredOptions[i].ToCharArray()[0]== quotationMark && filteredOptions[i].ToCharArray()[numberOfCharacter - 2] != quotationMark)
            {
                continuing = true;
                this.optionList[optionIndex].option.Add(filteredOptions[i]);
            }else if(continuing ==true && filteredOptions[i].ToCharArray()[numberOfCharacter-2] != quotationMark)
            {
                this.optionList[optionIndex].option.Add(filteredOptions[i]);
            }
            else if(continuing == true && filteredOptions[i].ToCharArray()[numberOfCharacter - 2] == quotationMark )
            {
                this.optionList[optionIndex].option.Add(filteredOptions[i]);
                continuing = false;
                optionIndex += 1;
            }
            else if (continuing == false && filteredOptions[i].ToCharArray()[0] == quotationMark && filteredOptions[i].ToCharArray()[numberOfCharacter - 2] == quotationMark)
            {
                this.optionList[optionIndex].option.Add(filteredOptions[i]);
                optionIndex += 1;
            }
        }
        startpos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        //if (tasksuccess == Task.Fail) npcmanager.myNPC = null;
        //if (hasdestination) {

        //}
    }
    public void Transitioninator() {
        print("yes");
        // starts transition between levels
        followplayer = false;
        player.GetComponent<Rigidbody>().isKinematic = false;
        //GetComponent<Collider>().enabled = true;
        tasksuccess = Task.Success;
        StartCoroutine(Transition(levelManager.level)); // does the actual transition, bus moves to train station/ player leaves train
        levelManager.taskcompletescreen.SetActive(true);
        player.inputtext.SetActive(false);

        levelManager.upgradeText.SetActive(true);
        levelManager.boost.GetComponentInChildren<TMP_Text>().text = "max increased";
        levelManager.boost.GetComponentInChildren<Image>().enabled = true;
        player.maxenergy *= 1.5f;
        player.energy += player.maxenergy * .2f;
        //if (player.energygain < player.maxEnergyGain) {
        //    player.energygain *= 2;
        //    print("gains");
        //}
        AudioManager.instance.PlaySFX(correctsound);
    }
    IEnumerator Transition(LevelManager.Level level) {
        if (level == LevelManager.Level.Bus && street != null) {// called by bus.cs
            street.bus.gameObject.SetActive(true);
            cam.target = street.campos;
            cam.bus = true;
            gameObject.SetActive(false);
            player.gameObject.SetActive(false);
        } 
        else if (level == LevelManager.Level.MRT) { // called by interactable.cs
            yield return new WaitForSeconds(1f);
            levelManager.Move(1,LevelManager.Level.Bus);
        }
        else if(level == LevelManager.Level.BusInterior) {// called by interactable.cs
            levelManager.Move(3, LevelManager.Level.MRT);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Player>() && !npcmanager.myNPC) {
            UpdateCanvas();
            player.canMove = false;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            levelManager.dialoguescreen.SetActive(true);
            //player.avatar.gameObject.SetActive(true);
            //avatar.gameObject.SetActive(true);
            //CameraPan();
            StartDialogue();
        }
    }
    //private void OnCollisionEnter(Collision collision) {
    //    if (collision.gameObject.GetComponent<Obstacle>()) { 
    //        gameObject.SetActive(false);
    //        levelManager.gameover = true;
    //    }
    //}
    void StartDialogue() {
        player.NPC = true;
        dialoguebox.SetActive(true);
        avatar.sprite = dialogueSprite;
        dialoguetext.text = "";
        currentline = 0;
        npcmanager.myNPC = GetComponent<NPC>();
        dialogueco = StartCoroutine(Dialogue());
    }
   
    public void EndDialogue() {
        dialoguebox.SetActive(false);
        player.canMove = true;
        //cam.target = player.transform;
        //cam.NPC = false;
        //if (levelManager.level == LevelManager.Level.Bus || levelManager.level == LevelManager.Level.BusInterior) cam.transform.position = cam.originalposition.position;
        //else cam.transform.position = cam.trainposition.position;
        //cam.smoothing = 3;
        //spoken = true;
        player.NPC = false;
        levelManager.dialoguescreen.SetActive(false);
        //player.avatar.gameObject.SetActive(false);
        //avatar.gameObject.SetActive(false);
        //dialoguetext.text = "";
        //if (!spoken) { // ensures spokencount is only increased once
        //    spoken = true;
        //    npcmanager.spokencount += 1;
        //}
    }

    public void Stop()
    {
        StopCoroutine(dialogueco);
    }
    public IEnumerator Dialogue() {
        foreach (char chr in dialogue[currentline]) { // types out dialogue character by character
            dialoguetext.text += chr;
            AudioManager.instance.PlaySFX(dialoguesound);
            yield return new WaitForSeconds(wordspeed);
        }
    }
    //void CameraPan() {
    //    cam.target = transform.GetChild(0);
    //    cam.NPC = true;
    //    player.NPC = true;
    //    cam.smoothing = 4f;
    //    cam.transform.position = Vector3.Lerp(transform.position, cam.targetposition, Time.deltaTime * cam.smoothing);
    //}
    public void FollowPlayer() {
        if (!followplayer) return;
        if (levelManager.level == LevelManager.Level.Bus) player.GetComponent<Rigidbody>().isKinematic = true;
        else Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>());
        transform.SetParent(null);
        Vector3 dir = (player.transform.position - transform.position);
        //GetComponent<Collider>().enabled = false;
        transform.Translate(movespeed * Time.deltaTime * dir);
    }
    void UpdateCanvas() {
        List<int> locationIndexs = new List<int>(0);
        for (int i = 0; i < questionLocation.Length; i++) {
            sub = questionLocation[i].Substring(0, 2);
            temp = npcLocation.Substring(0, 2);
            print(temp);
            print(sub);
            if (sub == temp) locationIndexs.Add(i);

        }
        int qnindex;
        if (locationIndexs.Count == 0) {
            Debug.LogError("No valid question with the assigned Location");
        }
        int index = Random.Range(0, locationIndexs.Count);
        qnindex = locationIndexs[index];
        hasdestination = false;
        string subOutcome = outcomes[qnindex].Substring(0, 6);
        string tempOutcome = "Start task".Substring(0, 6);
        print(subOutcome);
        print(tempOutcome);
        if (subOutcome == tempOutcome)
        {
            hasdestination = true;
        }


        levelManager.optionAButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        levelManager.optionBButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        levelManager.optionCButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        levelManager.optionDButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        foreach (int i in answer[qnindex].element) {
            if (i == 1) {
                levelManager.optionAButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
            } else if (i == 2) {
                levelManager.optionBButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
            } else if (i == 3) {
                levelManager.optionCButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
            } else if (i == 4) {
                levelManager.optionDButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
            }
        }
        dialogue = new string[optionList[qnindex].Dialogue.Count];
        for (int i = 0; i < optionList[qnindex].Dialogue.Count; i++) {
            dialogue[i] = optionList[qnindex].Dialogue[i];
        }
        optionA = optionList[qnindex].option[0];
        optionB = optionList[qnindex].option[1];
        if (optionList[qnindex].option.Count >= 3) optionC = this.optionList[qnindex].option[2];
        if (optionList[qnindex].option.Count >= 4) optionD = this.optionList[qnindex].option[3];
        nametext.text = names[Random.Range(0, names.Length)];
        questiontext.text = questions[qnindex];
        explaintext.text = explains[qnindex];
        optionAtext.text = "a)" + optionA;
        optionBtext.text = "b)" + optionB;
        if (optionList[qnindex].option.Count >= 3) optionCtext.text = "c)" + optionC;
        else {
            optionCtext.text = "";
            levelManager.optionCButton.SetActive(false);
        }
        if (optionList[qnindex].option.Count >= 4) optionDtext.text = "d)" + optionD;
        else {
            optionDtext.text = "";
            levelManager.optionDButton.SetActive(false);
        }
    }
}
[System.Serializable]
public class OptionsOfQuestions
{
    public List<string> option= new List<string>();
    public List<string> Dialogue= new List<string>();
}
[System.Serializable]
public class Answer
{
    public int[] element= new int[2];
}