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
    NPCManagement npcmanager;
    string[] names,questions,explains;
    //public Dictionary<TMP_Text,string> options = new Dictionary<TMP_Text,string>();
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CameraController>();
        player = FindObjectOfType<Player>();
        names = File.ReadAllLines("Assets\\Misc\\first-names.txt");
        questions = File.ReadAllLines("Assets\\Misc\\questions.txt");
        explains = File.ReadAllLines("Assets\\Misc\\explanations.txt");
        int qnindex = Random.Range(0,questions.Length);
        question = questions[qnindex];
        explain = explains[qnindex];
        NPCname = names[Random.Range(0, names.Length)];
        nametext.text = NPCname;
        questiontext.text = question;
        explaintext.text = explain;
        optionAtext.text = "a)" + optionA;
        optionBtext.text = "b)" + optionB;
        optionCtext.text = "c)" + optionC;
        optionDtext.text = "d)" + optionD;
        npcmanager = FindObjectOfType<NPCManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Player>() && !spoken) {
            player.canMove = false;
            CameraPan();
            StartDialogue();
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
}
