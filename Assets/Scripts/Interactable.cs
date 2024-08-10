using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Interactable : MonoBehaviour
{
    public Player player;
    protected NPCManagement npcmanager;
    protected LevelManager levelManager;
    public ObjectPool objectPool;
    [SerializeField]Vector3 trainsize;
    [SerializeField] int location;
    bool touchingPlayer;
    public bool unconditional; // does object transition unconditionally?
    //public GameObject[] L1Destinations, L2Destinations, L3Destinations;
    //public GameObject[] destinations;
    //public string location;
    //public bool target = false;
    //float radius;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = FindObjectOfType<Player>();
        npcmanager = FindObjectOfType<NPCManagement>();
        levelManager = FindObjectOfType<LevelManager>();
        touchingPlayer = false;
        if (levelManager.level == LevelManager.Level.MRT) transform.localScale = trainsize;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(touchingPlayer && Input.GetKeyDown(KeyCode.F)) {
            if (gameObject.CompareTag("Transition")) {
                levelManager.guidebox.SetActive(false);
                
                // runs regardless of if have NPC
                //if (levelManager.level == LevelManager.Level.Bus) {
                //    StartCoroutine(levelManager.Move(3, LevelManager.Level.MRT)); // go from bus to mrt
                //}
                //if (levelManager.level == LevelManager.Level.BusInterior) {
                //    StartCoroutine(levelManager.Move(1, LevelManager.Level.Bus)); // go from bus interior to bus
                //}
                if(unconditional){
                    // runs regardless of if have NPC
                    if (levelManager.level == LevelManager.Level.Bus) {
                        StartCoroutine(levelManager.Move(3, LevelManager.Level.MRT)); // go from bus to mrt
                    }
                    if (levelManager.level == LevelManager.Level.BusInterior) {
                        StartCoroutine(levelManager.Move(1, LevelManager.Level.Bus)); // go from bus interior to bus
                    }
                }
                else if (npcmanager.myNPC != null && (int)npcmanager.myNPC.dialogueData.dialogueQuestions[npcmanager.myNPC.qnindex].outcomeLocation == location) { // bus to bus interior, mrt to bus
                    if (levelManager.level == LevelManager.Level.MRT) { // called by interactable.cs go from mrt to bus
                        StartCoroutine(levelManager.Move(1, LevelManager.Level.Bus));
                    }
                    else npcmanager.myNPC.Transitioninator();
                    //gameObject.SetActive(false);
                }
            } 
            else { // for inside bus when you have to bring NPC somewhere but no transition happens
                if (npcmanager.myNPC != null && (int)npcmanager.myNPC.dialogueData.dialogueQuestions[npcmanager.myNPC.qnindex].outcomeLocation == location) { // bus to bus interior, mrt to bus
                    levelManager.taskCompleteImg.sprite = levelManager.taskCompletionPanelSprites[1];
                    levelManager.taskcompletescreen.SetActive(true);
                    npcmanager.myNPC.tasksuccess = NPC.Task.Success;
                    player.inputtext.SetActive(false);
                    gameObject.SetActive(false);
                    //AudioManager.instance.PlaySFX(correctsound);
                }
            }
        }
    }
    //private void OnTriggerStay(Collider other) {
    //    if (other.GetComponent<Player>()) {

    //    }
    //}
    protected virtual void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Player>()) {
            touchingPlayer = true;
            if (gameObject.CompareTag("Transition")) {
                if (gameObject.name.Contains("Train Station")) { // stop player if touching mrt station even if no NPC
                    player.canMove = false;
                    player.inputtext.SetActive(true);
                }
                if (npcmanager.myNPC != null && (int)npcmanager.myNPC.dialogueData.dialogueQuestions[npcmanager.myNPC.qnindex].outcomeLocation == location) { // stop player no matter what if have NPC
                    player.canMove = false;
                    player.inputtext.SetActive(true);
                    if (GetComponentInParent<RoadTile>()) npcmanager.myNPC.street = GetComponentInParent<RoadTile>();
                } 
                else /*if(npcmanager.myNPC == null)*/
                    {
                    if (levelManager.level == LevelManager.Level.BusInterior) { // dont stop player if in bus 
                        player.inputtext.SetActive(true);
                        GetComponentInChildren<Canvas>().transform.GetChild(0).gameObject.SetActive(true);
                    }

                }
            } 
            else {
                if (levelManager.level == LevelManager.Level.BusInterior && npcmanager.myNPC != null && (int)npcmanager.myNPC.dialogueData.dialogueQuestions[npcmanager.myNPC.qnindex].outcomeLocation == location) {
                    //player.canMove = false;
                    player.inputtext.SetActive(true);
                    //if (GetComponentInParent<RoadTile>()) npcmanager.myNPC.street = GetComponentInParent<RoadTile>();
                }
            }
           
        }
    }
    protected void OnTriggerExit(Collider other) {
        if (other.GetComponent<Player>()) 
        {
            touchingPlayer = false;
            player.inputtext.SetActive(false);
            player.canMove = true;
            if(levelManager.level == LevelManager.Level.BusInterior && gameObject.CompareTag("Transition")) {
                GetComponentInChildren<Canvas>().transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
