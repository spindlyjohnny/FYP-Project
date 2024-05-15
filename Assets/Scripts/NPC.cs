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
    public string NPCname,question,explain,optionA,optionB,optionC,optionD;
    public TMP_Text dialoguetext,questiontext,explaintext,optionAtext,optionBtext,optionCtext,optionDtext;
    public TMP_Text nametext;
    public float wordspeed;
    public int currentline;
    bool spoken;
    public bool followplayer;
    public NPCManagement npcmanager;
    string[] names, questions, explains;
    public string[] options;
    public List<string> filteredOptions= new List<string>(0);
    public List<OptionsOfQuestions> optionList;
    public TextAsset optionsFile;
    public float movespeed;
    LevelManager levelManager;
    public Vector3 startpos;
    public enum Task { Success,Fail,Default}
    public Task tasksuccess;
    public bool hasdestination; // set in inspector if NPC has a destination.
    public Coroutine dialogueco;
    [SerializeField]AudioClip dialoguesound;
    bool upgraded;
    [SerializeField]RoadTile street;
    //ThisIsSoStupid<List<string>> myoptions;

    //[System.Serializable]
    //class ThisIsSoStupid {
    //    public List<object> myoptions;

    //}
    //Rigidbody rb;
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
        
        for (int i = 0; i < options.Length; i++)
        {
            if(options[i] != options[1])
            {
                filteredOptions.Add(options[i]);
            }
        }
        bool continuing = false;
        int optionList = 0;
        char quotationMark = filteredOptions[0].ToCharArray()[0];        
        for(int i = 0; i < filteredOptions.Count; i++)
        {
            int numberOfCharacter = filteredOptions[i].ToCharArray().Length;

            if (continuing==false && filteredOptions[i].ToCharArray()[0]== quotationMark)
            {
                continuing = true;
                this.optionList[optionList].option.Add(filteredOptions[i]);
            }else if(continuing ==true && filteredOptions[i].ToCharArray()[numberOfCharacter-2] != quotationMark)
            {
                this.optionList[optionList].option.Add(filteredOptions[i]);
            }
            else if(continuing == true && filteredOptions[i].ToCharArray()[numberOfCharacter - 2] == quotationMark)
            {
                this.optionList[optionList].option.Add(filteredOptions[i]);
                continuing = false;
                optionList += 1;
            }
        }


        //print(options[0]);
        //ThisIsSoStupid loadedWrapper = JsonUtility.FromJson<ThisIsSoStupid>(options);
        //List<object> loadedList = loadedWrapper.myoptions;
        //print(loadedList[0]);
        int qnindex = Random.Range(0,questions.Length);
        //print(qnindex);
        //question = questions[qnindex];
        //explain = explains[qnindex];
        //optionA = optionList[qnindex].option[0];
        //optionB = optionList[qnindex].option[1];
        //if (optionList[qnindex].option[2] != null)optionC = this.optionList[qnindex].option[2];
        //if (optionList[qnindex].option[3] != null) optionD = this.optionList[qnindex].option[3];
        NPCname = names[Random.Range(0, names.Length)];
        nametext.text = NPCname;
        questiontext.text = question;
        explaintext.text = explain;
        optionAtext.text = "a)" + optionA;
        optionBtext.text = "b)" + optionB;
        optionCtext.text = "c)" + optionC;
        optionDtext.text = "d)" + optionD;
        upgraded = false;
        startpos = transform.localPosition;
        //rb = GetComponent<Rigidbody>();
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
                followplayer = false;
                //GetComponent<Collider>().enabled = true;
                tasksuccess = Task.Success;
                Transition(levelManager.level);
                levelManager.taskcompletescreen.SetActive(true);
                if (!upgraded) {
                    player.energygain = 20;
                    upgraded = true;
                }
                // play some sound.
            }
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Player>() && !spoken) {
            player.canMove = false;
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
        print("yes");
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
        transform.SetParent(null);
        Vector3 dir = (player.transform.position - transform.position);
        //GetComponent<Collider>().enabled = false;
        transform.Translate(movespeed * Time.deltaTime * dir);
    }
    void Transition(LevelManager.Level level) {
        if (level == LevelManager.Level.Bus) {
            street.bus.gameObject.SetActive(true);
            cam.target = street.campos;
            player.canMove = false;
            cam.bus = true;
            transform.SetParent(street.bus.transform);
            transform.position = street.bus.passengerpos.position;
            player.transform.SetParent(street.bus.transform);
            player.transform.position = street.bus.passengerpos.position;
            //StartCoroutine(street.bus.BusTransitioninator());
            if (street.bus.transitioned) {
                transform.SetParent(null);
                player.transform.SetParent(null);
                player.canMove = true;
                cam.bus = false;
                cam.target = player.transform;
                levelManager.level = LevelManager.Level.MRT;
            }
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