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
    NPCManagement npcmanager;
    string[] names, questions, explains;
    string options;
    public float movespeed;
    LevelManager levelManager;
    public Vector3 startpos;
    public enum Task { Success,Fail,Default}
    public Task tasksuccess;
    public Collider destination; // set in inspector if NPC has a destination.
    public Coroutine dialogueco;
    public int mycredits; // set in inspector
    [SerializeField]AudioClip dialoguesound;
    //ThisIsSoStupid<List<string>> myoptions;

    //[System.Serializable]
    //class ThisIsSoStupid {
    //    public List<object> myoptions;

    //}
    //Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CameraController>();
        player = FindObjectOfType<Player>();
        names = File.ReadAllLines("Assets\\Misc\\first-names.txt");
        questions = File.ReadAllLines("Assets\\Misc\\questions.txt");
        explains = File.ReadAllLines("Assets\\Misc\\explanations.txt");
        tasksuccess = Task.Default;
        //options = File.ReadAllText("Assets\\Misc\\options.json");
        //print(options[0]);
        //ThisIsSoStupid loadedWrapper = JsonUtility.FromJson<ThisIsSoStupid>(options);
        //List<object> loadedList = loadedWrapper.myoptions;
        //print(loadedList[0]);
        //int qnindex = Random.Range(0,questions.Length);
        //question = questions[qnindex];
        //explain = explains[qnindex];
        NPCname = names[Random.Range(0, names.Length)];
        nametext.text = NPCname;
        questiontext.text = question;
        explaintext.text = explain;
        optionAtext.text = "a)" + optionA;
        optionBtext.text = "b)" + optionB;
        optionCtext.text = "c)" + optionC;
        optionDtext.text = "d)" + optionD;
        npcmanager = FindObjectOfType<NPCManagement>();
        levelManager = FindObjectOfType<LevelManager>();
        startpos = transform.position;
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        if (destination != null) {
            var detector = Physics.OverlapSphere(transform.position, .1f);
            bool target = false;
            for(int i = 0; i < detector.Length; i++) {
                if (detector[i].CompareTag("Finish")) target = true;
            }
            if (target && Input.GetKeyDown(KeyCode.F)) {
                followplayer = false;
                //GetComponent<Collider>().enabled = true;
                tasksuccess = Task.Success;
                levelManager.taskcompletescreen.SetActive(true);
                levelManager.credits += mycredits;
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
        Vector3 dir = (player.transform.position - transform.position);
        //GetComponent<Collider>().enabled = false;
        transform.Translate(movespeed * Time.deltaTime * dir);
    }
}
