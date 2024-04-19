using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NPC : MonoBehaviour
{
    
    public GameObject dialoguebox;
    CameraController cam;
    Player player;
    public string[] dialogue;
    public string NPCname;
    public TMP_Text dialoguetext;
    public TMP_Text nametext;
    public float wordspeed;
    public int currentline;
    //public Transform camPos;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CameraController>();
        player = FindObjectOfType<Player>();
        nametext.text = NPCname;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Player>()) {
            player.canMove = false;
            //CameraPan();
            StartDialogue();
        }
    }
    void StartDialogue() {
        dialoguebox.SetActive(true);
        dialoguetext.text = "";
        currentline = 0;
        StartCoroutine(Dialogue());
    }
    public void ContinueDialogue() {
        if (dialogue.Length == 0) return;
        if (!gameObject.activeSelf) return;
        if (dialoguetext.text.Length >= dialogue[currentline].Length) { // check if all the text in the current line has been typed out.
            if (currentline < dialogue.Length - 1) { //check if there's more dialogue to type out
                currentline += 1; // go to next line
                dialoguetext.text = ""; // reset text
                StartCoroutine(Dialogue());
            } else {
                EndDialogue();
            }
        }
    }
    void EndDialogue() {
        dialoguebox.SetActive(false);
        //dialoguetext.text = "";
        //if (!spoken) { // ensures spokencount is only increased once
        //    spoken = true;
        //    npcmanager.spokencount += 1;
        //}
    }
    IEnumerator Dialogue() {
        //AudioManager.instance.RandomiseSFX(sfx);
        foreach (char chr in dialogue[currentline]) { // types out dialogue character by character
            dialoguetext.text += chr;
            yield return new WaitForSeconds(wordspeed);
        }
    }
    void CameraPan() {
        cam.target = transform;
        cam.targetposition = player.transform.position;//camPos.position;
        cam.transform.position = Vector3.Lerp(transform.position, cam.targetposition, Time.deltaTime * cam.smoothing);
    }
}
