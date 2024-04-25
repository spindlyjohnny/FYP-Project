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
    string[] names,questions,explains;
    public float movespeed;
    LevelManager levelManager;
    public Vector3 startpos;
    //Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CameraController>();
        player = FindObjectOfType<Player>();
        names = File.ReadAllLines("Assets\\Misc\\first-names.txt");
        //questions = File.ReadAllLines("Assets\\Misc\\questions.txt");
        //explains = File.ReadAllLines("Assets\\Misc\\explanations.txt");
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
        startpos = transform.localPosition;
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        if (Input.GetKeyDown(KeyCode.F)) {
            followplayer = false;
            // play some sound.
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Player>() && !spoken) {
            player.canMove = false;
            CameraPan();
            StartDialogue();
        }
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<Vehicle>()) { 
            gameObject.SetActive(false);
            levelManager.gameover = true;
        }
    }
    void StartDialogue() {
        dialoguebox.SetActive(true);
        dialoguetext.text = "";
        currentline = 0;
        npcmanager.myNPC = this;
        StartCoroutine(Dialogue());
    }
   
    public void EndDialogue() {
        dialoguebox.SetActive(false);
        player.canMove = true;
        cam.target = player.transform;
        cam.NPC = false;
        cam.transform.position = cam.originalposition.position;
        cam.smoothing = 1;
        spoken = true;
        //dialoguetext.text = "";
        //if (!spoken) { // ensures spokencount is only increased once
        //    spoken = true;
        //    npcmanager.spokencount += 1;
        //}
    }
    public IEnumerator Dialogue() {
        //AudioManager.instance.RandomiseSFX(sfx);
        foreach (char chr in dialogue[currentline]) { // types out dialogue character by character
            dialoguetext.text += chr;
            yield return new WaitForSeconds(wordspeed);
        }
    }
    void CameraPan() {
        cam.target = transform.GetChild(0);
        cam.NPC = true;
        cam.smoothing = 2f;
        cam.transform.position = Vector3.Lerp(transform.position, cam.targetposition, Time.deltaTime * cam.smoothing);
    }
    public void FollowPlayer() {
        if (!followplayer) return;
        Vector3 dir = (player.transform.position - transform.position);
        //rb.velocity = dir * movespeed;
        transform.Translate(movespeed * Time.deltaTime * dir);
        //transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * movespeed);
    }
}
