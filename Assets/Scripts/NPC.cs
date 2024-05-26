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
    public string[] dialogue;
    public string optionA,optionB,optionC,optionD;
    public TMP_Text dialoguetext,questiontext,explaintext,optionAtext,optionBtext,optionCtext,optionDtext;
    public TMP_Text nametext;
    public float wordspeed;
    public int currentline;
    bool spoken;
    public bool followplayer;
    public NPCManagement npcmanager;
    string[] names, questions, explains;
    string[] options;
    List<string> filteredOptions= new List<string>(0);
    public List<OptionsOfQuestions> optionList;
    public TextAsset optionsFile, dialogueFile, nameFile,questionFile;
    public float movespeed;
    LevelManager levelManager;
    public Vector3 startpos;
    public enum Task { Success,Fail,Default}
    public Task tasksuccess;
    public bool hasdestination; // set in inspector if NPC has a destination.
    public Coroutine dialogueco;
    [SerializeField]AudioClip dialoguesound, correctsound;
    bool upgraded;
    [SerializeField]RoadTile street;
    public Answer[] answer = new Answer[51];
    // Start is called before the first frame update
    void Start()
    {
        npcmanager = FindObjectOfType<NPCManagement>();
        levelManager = FindObjectOfType<LevelManager>();
        cam = FindObjectOfType<CameraController>();
        player = FindObjectOfType<Player>();
        names = File.ReadAllLines("Assets\\Misc\\first-names.txt");
        questions = File.ReadAllLines("Assets\\Misc\\questions.txt");
        explains = File.ReadAllLines("Assets\\Misc\\explanations.txt");
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
        //turning the visibility on
        levelManager.optionCButton.SetActive(true);
        levelManager.optionDButton.SetActive(true);

        for (int i = 0; i < options.Length; i++)
        {
            if(options[i] != options[1])
            {
                filteredOptions.Add(options[i]);
            }
        }
        bool continuing = false;
        int optionIndex = 0;
        char quotationMark = filteredOptions[0].ToCharArray()[0];        
        for(int i = 0; i < filteredOptions.Count; i++)
        {
            int numberOfCharacter = filteredOptions[i].ToCharArray().Length;

            if (continuing==false && filteredOptions[i].ToCharArray()[0]== quotationMark)
            {
                continuing = true;
                this.optionList[optionIndex].option.Add(filteredOptions[i]);
            }else if(continuing ==true && filteredOptions[i].ToCharArray()[numberOfCharacter-2] != quotationMark)
            {
                this.optionList[optionIndex].option.Add(filteredOptions[i]);
            }
            else if(continuing == true && filteredOptions[i].ToCharArray()[numberOfCharacter - 2] == quotationMark)
            {
                this.optionList[optionIndex].option.Add(filteredOptions[i]);
                continuing = false;
                optionIndex += 1;
            }
        }

        upgraded = false;
        startpos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        if (hasdestination) {
            var detector = Physics.OverlapSphere(transform.position, .5f);
            bool target = false;
            //MRTTile mrt;
            for(int i = 0; i < detector.Length; i++) {
                if (detector[i].CompareTag("Finish")) target = true;
                if (detector[i].GetComponent<RoadTile>()) {
                    street = detector[i].GetComponent<RoadTile>();
                }
                //else if(detector[i]) // mrt tile.
            }
            if (target && Input.GetKeyDown(KeyCode.F)) {
                print("yes");
                followplayer = false;
                player.GetComponent<Rigidbody>().isKinematic = false;
                //GetComponent<Collider>().enabled = true;
                tasksuccess = Task.Success;
                Transition(levelManager.level);
                levelManager.taskcompletescreen.SetActive(true);
                levelManager.taskcompletescreen.transform.Find("Upgrade Text").gameObject.SetActive(true);
                if (!upgraded) {
                    player.energygain = 20;
                    upgraded = true;
                }
                AudioManager.instance.PlaySFX(correctsound);
            }
            if (street != null && street.bus.transitioned) {
                transform.SetParent(null);
                player.transform.SetParent(null);
                player.GetComponent<Collider>().enabled = true;
                //foreach (var i in GetComponents<Collider>()) i.enabled = true;
                player.canMove = true;
                cam.bus = false;
                cam.target = player.transform;
                cam.transform.position = cam.originalposition.position;
                levelManager.level = LevelManager.Level.MRT;
                //cam.train = true;
                //levelManager.Spawn(1);
                Destroy(gameObject);
            }
        }
    }

    void UpdateCanvas()
    {

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
        nametext.text =names[Random.Range(0, names.Length)];
        questiontext.text =  questions[qnindex];
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

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Player>() && !spoken && !npcmanager.myNPC) {
            UpdateCanvas();
            player.canMove = false;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            CameraPan();
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
        dialoguebox.SetActive(true);
        dialoguetext.text = "";
        currentline = 0;
        npcmanager.myNPC = this;
        dialogueco = StartCoroutine(Dialogue());
    }
   
    public void EndDialogue() {
        dialoguebox.SetActive(false);
        player.canMove = true;
        cam.target = player.transform;
        cam.NPC = false;
        cam.transform.position = cam.originalposition.position;
        cam.smoothing = 3;
        spoken = true;
        player.NPC = false;
        //dialoguetext.text = "";
        //if (!spoken) { // ensures spokencount is only increased once
        //    spoken = true;
        //    npcmanager.spokencount += 1;
        //}
    }
    public IEnumerator Dialogue() {
        foreach (char chr in dialogue[currentline]) { // types out dialogue character by character
            dialoguetext.text += chr;
            AudioManager.instance.PlaySFX(dialoguesound);
            yield return new WaitForSeconds(wordspeed);
        }
    }
    void CameraPan() {
        cam.target = transform.GetChild(0);
        cam.NPC = true;
        player.NPC = true;
        cam.smoothing = 4f;
        cam.transform.position = Vector3.Lerp(transform.position, cam.targetposition, Time.deltaTime * cam.smoothing);
    }
    public void FollowPlayer() {
        if (!followplayer) return;
        player.GetComponent<Rigidbody>().isKinematic = true;
        transform.SetParent(null);
        Vector3 dir = (player.transform.position - transform.position);
        //GetComponent<Collider>().enabled = false;
        transform.Translate(movespeed * Time.deltaTime * dir);
    }
    void Transition(LevelManager.Level level) {
        if (level == LevelManager.Level.Bus) {
            street.bus.gameObject.SetActive(true);
            street.station.SetActive(true);
            cam.target = street.campos;
            player.canMove = false;
            cam.bus = true;
            transform.SetParent(street.bus.transform);
            transform.position = street.bus.passengerpos.position;
            player.transform.SetParent(street.bus.transform);
            player.GetComponent<Collider>().enabled = false;
            foreach (var i in GetComponents<Collider>()) i.enabled = false;
            player.transform.position = street.bus.passengerpos.position;
            player.inputtext.SetActive(false);
            //StartCoroutine(street.bus.BusTransitioninator());
            //if (street.bus.transitioned) {
            //    transform.SetParent(null);
            //    player.transform.SetParent(null);
            //    player.GetComponent<Collider>().enabled = true;
            //    foreach(var i in GetComponents<Collider>())i.enabled = true;
            //    player.canMove = true;
            //    cam.bus = false;
            //    cam.target = player.transform;
            //    levelManager.level = LevelManager.Level.MRT;
            //}
        }
        else if(level == LevelManager.Level.MRT) {

        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, .5f);
    }
}
[System.Serializable]
public class OptionsOfQuestions
{
    public List<string> option= new List<string>();
}
[System.Serializable]
public class Answer
{
    public int[] element= new int[2];
}