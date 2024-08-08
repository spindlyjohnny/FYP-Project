using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
public class NPC : MonoBehaviour
{
    public GameObject dialoguebox, questionbox;
    CameraController cam;
    Player player;
    TutorialUI tutorial;
    public string[] dialogue = new string[0];
    public bool[] npcTalking = new bool[3];
    public List<string> dialogueList;
    public string optionA, optionB, optionC, optionD;
    public LocationEnum locationNpc;
    public TMP_Text dialoguetext, questiontext, explaintext, optionAtext, optionBtext, optionCtext, optionDtext;
    public TMP_Text nametext;
    public float wordspeed;
    public int currentline;
    //bool spoken;
    public bool followplayer, once = false;
    public NPCManagement npcmanager;
    //string[] names;
    public Names names;
    public DialogueSO dialogueData;
    public float movespeed;
    LevelManager levelManager;
    public Vector3 startpos;
    public enum Task { Success, Fail, Default }
    public Task tasksuccess;
    public bool hasdestination; // set in inspector if NPC has a destination.
    public Coroutine dialogueco;
    [SerializeField] AudioClip dialoguesound, correctsound;
    public RoadTile street;
    public string sub;
    public string temp;
    public int character = 2;
    public Image avatar;
    public Sprite dialogueSprite;
    public int qnindex;
    public int indexDialogue = 0;
    [SerializeField] string nameFile;
    // Start is called before the first frame update
    void Start()
    {
        //setting the references
        npcmanager = FindObjectOfType<NPCManagement>();
        tutorial = FindObjectOfType<TutorialUI>();
        levelManager = FindObjectOfType<LevelManager>();
        cam = FindObjectOfType<CameraController>();
        player = FindObjectOfType<Player>();
        tasksuccess = Task.Default;
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
        names = GetComponent<Names>();
        avatar = levelManager.npcAvatar;
        //avatar.sprite = dialogueSprite;
        //turning the visibility on
        levelManager.optionCButton.SetActive(true);
        levelManager.optionDButton.SetActive(true);
        if (levelManager.level == LevelManager.Level.Bus)
        {
            locationNpc = LocationEnum.Pavement;
            //RaycastHit hit;
            //Physics.Raycast(transform.position, Vector3.down, out hit);
            //if (hit.collider.gameObject.CompareTag("Train")) {
            //    npcLocation = "Train";
            //}
        }
        else if (levelManager.level == LevelManager.Level.BusInterior)
        {
            locationNpc = LocationEnum.BusInterior;
        }
        else if (levelManager.level == LevelManager.Level.MRT)
        {
            locationNpc = LocationEnum.Mrt;
        }
        startpos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        if (TutorialUI.state != TutorialUI.TutorialState.fifthInstruction) return;
        if (Mathf.Abs(transform.position.z - player.transform.position.z) >= 0.5f) return;

    }
    public void Transitioninator()
    {
        print("yes");
        // starts transition between levels
        followplayer = false;
        //GetComponent<Collider>().enabled = true;
        tasksuccess = Task.Success;
        StartCoroutine(Transition(levelManager.level)); // does the actual transition, bus moves to train station/ player leaves train
        levelManager.taskcompletescreen.SetActive(true);
        player.inputtext.SetActive(false);

        //levelManager.upgradeText.SetActive(true);
        levelManager.boost.SetActive(true);
        levelManager.boost.GetComponentInChildren<TMP_Text>().text = "max increased";
        levelManager.taskCompleteImgResponse.sprite = levelManager.taskCompletionPanelSprites[3];
        levelManager.taskCompleteImg.sprite = levelManager.taskCompletionPanelSprites[3];
        player.maxenergy *= 1.5f;
        player.energy += player.maxenergy * .2f;
        //if (player.energygain < player.maxEnergyGain) {
        //    player.energygain *= 2;
        //    print("gains");
        //}
        AudioManager.instance.PlaySFX(correctsound);
    }
    IEnumerator Transition(LevelManager.Level level)
    {
        if (level == LevelManager.Level.Bus && street != null)
        {// called by bus.cs go from bus to bus interior
            street.bus.gameObject.SetActive(true);
            cam.target = street.campos;
            cam.bus = true;
            gameObject.SetActive(false);
            player.gameObject.SetActive(false);
        }
        else if (level == LevelManager.Level.MRT)
        { // called by interactable.cs go from mrt to bus
            yield return new WaitForSeconds((float)levelManager.loadingScreen.videoPlayer.length);
            StartCoroutine(levelManager.Move(1, LevelManager.Level.Bus));
        } /*else if (level == LevelManager.Level.BusInterior) {// called by interactable.cs
            levelManager.Move(3, LevelManager.Level.MRT);
        }*/
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() && !npcmanager.myNPC)
        {
            Assess();
            player.canMove = false;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            levelManager.dialoguescreen.SetActive(true);
            StartDialogue();
        }
    }
    //private void OnCollisionEnter(Collision collision) {
    //    if (collision.gameObject.GetComponent<Obstacle>()) { 
    //        gameObject.SetActive(false);
    //        levelManager.gameover = true;
    //    }
    //}
    public void StartDialogue()
    {
        player.NPC = true;
        dialoguebox.SetActive(true);
        avatar.sprite = dialogueSprite;
        dialoguetext.text = "";
        currentline = 0;
        npcmanager.myNPC = GetComponent<NPC>();
        dialogueco = StartCoroutine(Dialogue());
        //Time.timeScale = 0;
    }

    public void EndDialogue()
    {
        dialoguebox.SetActive(false);
        player.canMove = true;
        player.NPC = false;
        levelManager.dialoguescreen.SetActive(false);
    }

    public void Guide()
    {
        if (dialogueData.dialogueQuestions[qnindex].outcomeLocation == OutcomeLocation.busStop)
        {
            levelManager.guideText.text = "Ride the bus with the commuter";
        }
        else if(dialogueData.dialogueQuestions[qnindex].outcomeLocation== OutcomeLocation.emptySeat)
        {
            levelManager.guideText.text = "Find empty seats and leave at their stop";
        }
        else if(dialogueData.dialogueQuestions[qnindex].outcomeLocation == OutcomeLocation.busCaptain)
        {
            levelManager.guideText.text = "Ask the Bus Captain about the route";
        }
        else if (dialogueData.dialogueQuestions[qnindex].outcomeLocation == OutcomeLocation.grandKid)
        {
            levelManager.guideText.text ="Find the commuter's grandkid and leave at their stop";
        }
        else if (dialogueData.dialogueQuestions[qnindex].outcomeLocation == OutcomeLocation.stationStaff)
        {
            levelManager.guideText.text = "Bring commuter to the station staff";
        }
        else if (dialogueData.dialogueQuestions[qnindex].outcomeLocation == OutcomeLocation.taskCompletion)
        {
            return;
        }
        else if (dialogueData.dialogueQuestions[qnindex].outcomeLocation == OutcomeLocation.WheelchairZone)
        {
            levelManager.guideText.text = "Bring commuter to wheelchair zone and leave at their stop";
        }
        levelManager.guidebox.SetActive(true);
    }

    public void Stop()
    {
        StopCoroutine(dialogueco);
    }
    public IEnumerator Dialogue()
    {
        Time.timeScale = 0;
        if (npcTalking[currentline])
        {
            avatar.gameObject.SetActive(true);
            dialoguebox.transform.Find("NPC Name Boks").gameObject.SetActive(true);
            player.avatar.gameObject.SetActive(false);
            dialoguebox.transform.Find("Player Name Boks").gameObject.SetActive(false);
        }
        else
        {
            avatar.gameObject.SetActive(false);
            dialoguebox.transform.Find("NPC Name Boks").gameObject.SetActive(false);
            dialoguebox.transform.Find("Player Name Boks").gameObject.SetActive(true);
            player.avatar.gameObject.SetActive(true);
        }
        for (int i = 0; i < 5; i++)
        {
            AudioManager.instance.PlaySFX(dialoguesound);
            yield return new WaitForSecondsRealtime(wordspeed);
        }
        foreach (char chr in dialogue[currentline])
        { // types out dialogue character by character
            dialoguetext.text += chr;
            yield return new WaitForSecondsRealtime(wordspeed);
        }
    }
    public void FollowPlayer()
    {
        if (!followplayer) return;
        /*if (levelManager.level == LevelManager.Level.Bus) player.GetComponent<Rigidbody>().isKinematic = true;
        else*/ //Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>());
        transform.position = player.npcPosition.position;
        transform.SetParent(player.transform);
        if (once == false)
        {
            once = true;
            GetComponentInChildren<Image>().gameObject.SetActive(false);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
        }
        /*
    Vector3 dir = (player.transform.position - transform.position);
    Quaternion lookRotation = Quaternion.LookRotation(dir   );
    transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 180 * Time.deltaTime);*/
        //GetComponent<Collider>().enabled = false;
        //transform.Translate(movespeed * Time.deltaTime * dir);
    }

    public void Response(int index)
    {
        npcTalking = new bool[dialogueData.dialogueQuestions[qnindex].response[index].response.Length];
        for (int i = 0; i < dialogueData.dialogueQuestions[qnindex].response[index].response.Length; i++)
        {
            npcTalking[i] = true;
        }
        dialogue = new string[dialogueData.dialogueQuestions[qnindex].response[index].response.Length];
        for (int i = 0; i < dialogueData.dialogueQuestions[qnindex].response[index].response.Length; i++)
        {
            dialogue[i] = dialogueData.dialogueQuestions[qnindex].response[index].response[i];
        }
    }

    void Assess()
    {
        List<int> locationIndexs = new List<int>(0);
        for (int i = 0; i < dialogueData.dialogueQuestions.Length; i++)
        {
            //npcLocation = dialogueData.dialogueQuestions[i].location;
            if (locationNpc == dialogueData.dialogueQuestions[i].locationE) locationIndexs.Add(i);
        }
        /* for later use
        for (int i = 0; i < dialogueData.dialogueQuestions.Length; i++)
        {
            if (dialogueData.dialogueQuestions[i].locationE != locationNpc)
            {
                return;
            }
            locationIndexs.Add(i);
        }*/
        if (locationIndexs.Count == 0)
        {
            Debug.LogError("No valid question with the assigned Location");
        }
        int index = Random.Range(0, locationIndexs.Count);
        qnindex = locationIndexs[index];
        print(qnindex);

        levelManager.optionAButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        levelManager.optionBButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        levelManager.optionCButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        levelManager.optionDButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        if (dialogueData.dialogueQuestions[qnindex].assDial[0].isCorrect == true)
        {
            levelManager.optionAButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
        }
        if (dialogueData.dialogueQuestions[qnindex].assDial[1].isCorrect == true)
        {
            levelManager.optionBButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
        }
        if (dialogueData.dialogueQuestions[qnindex].assDial.Length >= 3 && dialogueData.dialogueQuestions[qnindex].assDial[2].isCorrect == true)
        {
            levelManager.optionCButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
        }
        if (dialogueData.dialogueQuestions[qnindex].assDial.Length == 4 && dialogueData.dialogueQuestions[qnindex].assDial[3].isCorrect == true)
        {
            levelManager.optionDButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
        }
        npcTalking = new bool[dialogueData.dialogueQuestions[qnindex].assDialogue.Length];
        for (int i = 0; i < dialogueData.dialogueQuestions[qnindex].assDialogue.Length; i++)
        {
            npcTalking[i] = dialogueData.dialogueQuestions[qnindex].assDialogue[i].npcTalking;
        }
        dialogue = new string[dialogueData.dialogueQuestions[qnindex].assDialogue.Length];
        for (int i = 0; i < dialogueData.dialogueQuestions[qnindex].assDialogue.Length; i++)
        {
            dialogue[i] = dialogueData.dialogueQuestions[qnindex].assDialogue[i].speechLine;
        }
        nametext.text = names.names[Random.Range(0,names.names.Length)];
        questiontext.text = dialogueData.dialogueQuestions[qnindex].assessQuestion;
        optionAtext.text = "A. " + dialogueData.dialogueQuestions[qnindex].assDial[0].option;
        optionBtext.text = "B. " + dialogueData.dialogueQuestions[qnindex].assDial[1].option;
        levelManager.optionCButton.SetActive(true);
        levelManager.optionDButton.SetActive(true);
        if (dialogueData.dialogueQuestions[qnindex].assDial.Length >= 3) optionCtext.text = "C. " + dialogueData.dialogueQuestions[qnindex].assDial[2].option;
        else
        {
            optionCtext.text = "";
            levelManager.optionCButton.SetActive(false);
        }
        if (dialogueData.dialogueQuestions[qnindex].assDial.Length >= 4) optionDtext.text = "D. " + dialogueData.dialogueQuestions[qnindex].assDial[3].option;
        else
        {
            optionDtext.text = "";
            levelManager.optionDButton.SetActive(false);
        }
    }

    public void UpdateCanvas()
    {
        hasdestination = false;
        if(dialogueData.dialogueQuestions[qnindex].outcomeLocation != OutcomeLocation.taskCompletion)
        {
            hasdestination = true;
        }


        levelManager.optionAButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        levelManager.optionBButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        levelManager.optionCButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        levelManager.optionDButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.WrongOption;
        if (dialogueData.dialogueQuestions[qnindex].Dial[0].isCorrect == true)
        {
            levelManager.optionAButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
        }
        if (dialogueData.dialogueQuestions[qnindex].Dial[1].isCorrect == true)
        {
            levelManager.optionBButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
        }
        if (dialogueData.dialogueQuestions[qnindex].Dial.Length == 3)
        {
            if (dialogueData.dialogueQuestions[qnindex].Dial[2].isCorrect == true)
            {
                levelManager.optionCButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
            }
        }
        if (dialogueData.dialogueQuestions[qnindex].Dial.Length == 4)
        {
            if (dialogueData.dialogueQuestions[qnindex].Dial[3].isCorrect == true)
            {
                levelManager.optionDButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
            }
        }
        npcTalking = new bool[dialogueData.dialogueQuestions[qnindex].Dialogues.Length];
        for (int i = 0; i < dialogueData.dialogueQuestions[qnindex].Dialogues.Length; i++)
        {
            npcTalking[i] = dialogueData.dialogueQuestions[qnindex].Dialogues[i].npcTalking;
        }
        dialogue = new string[dialogueData.dialogueQuestions[qnindex].Dialogues.Length];
        for (int i = 0; i < dialogueData.dialogueQuestions[qnindex].Dialogues.Length; i++)
        {
            dialogue[i] = dialogueData.dialogueQuestions[qnindex].Dialogues[i].speechLine;
        }
        questiontext.text = dialogueData.dialogueQuestions[qnindex].question;
        optionAtext.text = "A. " + dialogueData.dialogueQuestions[qnindex].Dial[0].option;
        optionBtext.text = "B. " + dialogueData.dialogueQuestions[qnindex].Dial[1].option;
        levelManager.optionCButton.SetActive(true);
        levelManager.optionDButton.SetActive(true);
        if (dialogueData.dialogueQuestions[qnindex].Dial.Length >= 3) optionCtext.text = "C. " + dialogueData.dialogueQuestions[qnindex].Dial[2].option;
        else
        {
            optionCtext.text = "";
            levelManager.optionCButton.SetActive(false);
        }
        if (dialogueData.dialogueQuestions[qnindex].Dial.Length >= 4) optionDtext.text = "D. " + dialogueData.dialogueQuestions[qnindex].Dial[3].option;
        else
        {
            optionDtext.text = "";
            levelManager.optionDButton.SetActive(false);
        }
    }
}
[System.Serializable]
public class OptionsOfQuestions
{
    public List<string> option = new List<string>();
    public List<string> Dialogue = new List<string>();
}
[System.Serializable]
public class Answer
{
    public int[] element = new int[2];
}