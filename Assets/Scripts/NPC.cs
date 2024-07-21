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
    public LocationEnum locationNpc;
    public TMP_Text dialoguetext,questiontext,explaintext,optionAtext,optionBtext,optionCtext,optionDtext;
    public TMP_Text nametext;
    public float wordspeed;
    public int currentline;
    //bool spoken;
    public bool followplayer,once=false;
    public NPCManagement npcmanager;
    string[] names;
    public DialogueSO dialogueData;
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
    int qnindex;
    public int indexDialogue = 0;
    [SerializeField]TextAsset nameFile;
    public float spawnYOffset;
    // Start is called before the first frame update
    void Start()
    {
        //setting the references
        npcmanager = FindObjectOfType<NPCManagement>();
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
        names = File.ReadAllLines("Assets/Misc/"+nameFile.name+".txt");
        avatar = levelManager.npcAvatar;
        //avatar.sprite = dialogueSprite;
        //turning the visibility on
        levelManager.optionCButton.SetActive(true);
        levelManager.optionDButton.SetActive(true);
        if(levelManager.level == LevelManager.Level.Bus) {
            npcLocation = "Pavement";
            //RaycastHit hit;
            //Physics.Raycast(transform.position, Vector3.down, out hit);
            //if (hit.collider.gameObject.CompareTag("Train")) {
            //    npcLocation = "Train";
            //}
        }
        else if(levelManager.level == LevelManager.Level.BusInterior) {
            npcLocation = "Inside Bus";
        }
        else if(levelManager.level == LevelManager.Level.MRT) {
            npcLocation = "Train";
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
        //GetComponent<Collider>().enabled = true;
        tasksuccess = Task.Success;
        StartCoroutine(Transition(levelManager.level)); // does the actual transition, bus moves to train station/ player leaves train
        levelManager.taskcompletescreen.SetActive(true);
        player.inputtext.SetActive(false);

        levelManager.upgradeText.SetActive(true);
        levelManager.boost.SetActive(true);
        levelManager.boost.GetComponentInChildren<TMP_Text>().text = "max increased";
        foreach (var i in levelManager.boost.GetComponentsInChildren<Image>()) i.enabled = true;
        //levelManager.boost.GetComponentInChildren<Image>().enabled = true;
        player.maxenergy *= 1.5f;
        player.energy += player.maxenergy * .2f;
        //if (player.energygain < player.maxEnergyGain) {
        //    player.energygain *= 2;
        //    print("gains");
        //}
        AudioManager.instance.PlaySFX(correctsound);
    }
    IEnumerator Transition(LevelManager.Level level) {
        if (level == LevelManager.Level.Bus && street != null) {// called by bus.cs go from bus to bus interior
            street.bus.gameObject.SetActive(true);
            cam.target = street.campos;
            cam.bus = true;
            gameObject.SetActive(false);
            player.gameObject.SetActive(false);
        } else if (level == LevelManager.Level.MRT) { // called by interactable.cs go from mrt to bus
            yield return new WaitForSeconds(1f);
            levelManager.Move(1, LevelManager.Level.Bus);
        } /*else if (level == LevelManager.Level.BusInterior) {// called by interactable.cs
            levelManager.Move(3, LevelManager.Level.MRT);
        }*/
    }
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Player>() && !npcmanager.myNPC) {
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
    public void StartDialogue() {
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
        player.NPC = false;
        levelManager.dialoguescreen.SetActive(false);
    }

    public void Stop()
    {
        StopCoroutine(dialogueco);
    }
    public IEnumerator Dialogue() {
        for (int i = 0; i < 5; i++) {
            AudioManager.instance.PlaySFX(dialoguesound);
            yield return new WaitForSeconds(wordspeed);
        }
        foreach (char chr in dialogue[currentline]) { // types out dialogue character by character
            dialoguetext.text += chr;
            yield return new WaitForSeconds(wordspeed);
        }
    }
    public void FollowPlayer() {
        if (!followplayer) return;
        /*if (levelManager.level == LevelManager.Level.Bus) player.GetComponent<Rigidbody>().isKinematic = true;
        else*/ //Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>());
        transform.position = player.npcPosition.position;
        transform.SetParent(player.transform);
        if (once == false)
        {
            once = true;
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
            sub = dialogueData.dialogueQuestions[i].location.Substring(0, 2);
            temp = npcLocation.Substring(0, 2);
            print(temp);
            print(sub);
            if (sub == temp) locationIndexs.Add(i);
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
        if (dialogueData.dialogueQuestions[qnindex].assessAnswer[0] == true)
        {
            levelManager.optionAButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
        }
        if (dialogueData.dialogueQuestions[qnindex].assessAnswer[1] == true)
        {
            levelManager.optionBButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
        }
        if (dialogueData.dialogueQuestions[qnindex].assessAnswer.Length >= 3 && dialogueData.dialogueQuestions[qnindex].assessAnswer[2] == true)
        {
            levelManager.optionCButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;            
        }
        if (dialogueData.dialogueQuestions[qnindex].assessAnswer.Length == 4 && dialogueData.dialogueQuestions[qnindex].assessAnswer[3] == true)
        {
            levelManager.optionDButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;            
        }

        dialogue = new string[dialogueData.dialogueQuestions[qnindex].assessDialogue.Length];
        for (int i = 0; i < dialogueData.dialogueQuestions[qnindex].assessDialogue.Length; i++)
        {
            dialogue[i] = dialogueData.dialogueQuestions[qnindex].assessDialogue[i];
        }
        nametext.text = names[Random.Range(0, names.Length)];
        questiontext.text = dialogueData.dialogueQuestions[qnindex].assessQuestion;
        optionAtext.text = "a)" + dialogueData.dialogueQuestions[qnindex].assessOption[0];
        optionBtext.text = "b)" + dialogueData.dialogueQuestions[qnindex].assessOption[1];
        levelManager.optionCButton.SetActive(true);
        levelManager.optionDButton.SetActive(true);
        if (dialogueData.dialogueQuestions[qnindex].assessOption.Length >= 3) optionCtext.text = "c)" + dialogueData.dialogueQuestions[qnindex].assessOption[2];
        else
        {
            optionCtext.text = "";
            levelManager.optionCButton.SetActive(false);
        }
        if (dialogueData.dialogueQuestions[qnindex].assessOption.Length >= 4) optionDtext.text = "d)" + dialogueData.dialogueQuestions[qnindex].assessOption[3];
        else
        {
            optionDtext.text = "";
            levelManager.optionDButton.SetActive(false);
        }
    }

    public void UpdateCanvas() {
        hasdestination = false;
        string subOutcome = dialogueData.dialogueQuestions[qnindex].outcome.Substring(0, 6);
        string tempOutcome = "Start task".Substring(0, 6);
        if (subOutcome == tempOutcome)
        {
            hasdestination = true;
        }


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
        if(dialogueData.dialogueQuestions[qnindex].answer.Length == 3) {
            if (dialogueData.dialogueQuestions[qnindex].answer[2] == true) {
                levelManager.optionCButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
            }  
        }
        if(dialogueData.dialogueQuestions[qnindex].answer.Length == 4) {
            if (dialogueData.dialogueQuestions[qnindex].answer[3] == true) {
                levelManager.optionDButton.GetComponent<NPCQuestion>().option = NPCQuestion.Options.CorrectOption;
            }
        }

        dialogue = new string[dialogueData.dialogueQuestions[qnindex].Dialogue.Length];
        for (int i = 0; i < dialogueData.dialogueQuestions[qnindex].Dialogue.Length; i++) {
            dialogue[i] = dialogueData.dialogueQuestions[qnindex].Dialogue[i];
        }
        if (dialogueData.dialogueQuestions[qnindex].options.Length >= 3) optionC = this.dialogueData.dialogueQuestions[qnindex].options[2];
        if (dialogueData.dialogueQuestions[qnindex].options.Length >= 4) optionD = this.dialogueData.dialogueQuestions[qnindex].options[3];
        questiontext.text = dialogueData.dialogueQuestions[qnindex].question;
        optionAtext.text = "a)" + dialogueData.dialogueQuestions[qnindex].options[0];
        optionBtext.text = "b)" + dialogueData.dialogueQuestions[qnindex].options[1];
        levelManager.optionCButton.SetActive(true);
        levelManager.optionDButton.SetActive(true);
        if (dialogueData.dialogueQuestions[qnindex].options.Length >= 3) optionCtext.text = "c)" + dialogueData.dialogueQuestions[qnindex].options[2];
        else {
            optionCtext.text = "";
            levelManager.optionCButton.SetActive(false);
        }
        if (dialogueData.dialogueQuestions[qnindex].options.Length >= 4) optionDtext.text = "d)" + dialogueData.dialogueQuestions[qnindex].options[3];
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