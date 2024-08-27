using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    Vector3 movement;
    [HideInInspector]
    public bool pushingWheelchair = false;
    public float movespeed, maxspeed;
    public bool canMove, NPC,interactable;
    LevelManager levelManager;
    TutorialUI tutorial;
    public float energy, maxenergy;
    public MeshRenderer[] meshes;
    public SkinnedMeshRenderer[] skin;
    public Color originalColor, hitColor;
    public AudioClip hitsfx;
    public GameObject inputtext;
    public bool invincibility = false;
    public float originalInvincibleTime, maxInvincibleTime;
    [SerializeField] float invincibilitytime, hitTime;
    public Tile tile;
    public Image avatar;
    public Sprite dialogueSprite, loseSprite, pauseSprite; // these vars exist cuz there's 2 playable charas
    public Animator anim;
    public int lane = 1, newlane;
    public Transform npcPosition, npcWheelchairPosition;
    bool animating = false;
    public Material[] invincibleMats;
    public Material[] originalMats;
    bool hit;
    public GeneralDIalogueSO generalData;
    public static List<int> list= new List<int>();
    public List<int> tempList= new List<int>();
    Rigidbody rb;
    [SerializeField] Transform lefthit, righthit;
    // Start is called before the first frame update
    private void Awake() {
        if (PlayerPrefs.GetInt("bool") == 1) {
            energy = PlayerPrefs.GetFloat("energy");
            maxenergy = PlayerPrefs.GetFloat("Max Energy");
            //energygain = PlayerPrefs.GetFloat("Energy Gain");
            originalInvincibleTime = PlayerPrefs.GetFloat("Invincibility Time");
        } else {
            PlayerPrefs.SetFloat("energy", 100);
            PlayerPrefs.SetFloat("Max Energy", 100);
            //PlayerPrefs.SetFloat("Energy Gain", 10f);
            PlayerPrefs.SetFloat("Invincibility Time", 5f);
            energy = PlayerPrefs.GetFloat("energy");
        }

    }
    void Start()
    {
        
        interactable = false;
        levelManager = FindObjectOfType<LevelManager>();
        canMove = true;
        invincibilitytime = originalInvincibleTime;
        avatar.sprite = dialogueSprite;
        levelManager.energyslider.maxValue = maxenergy;
        rb = GetComponent<Rigidbody>();
        meshes = GetComponentsInChildren<MeshRenderer>();
        skin = GetComponentsInChildren<SkinnedMeshRenderer>();
        if (FindObjectOfType<TutorialUI>())
        {
            tutorial = FindObjectOfType<TutorialUI>();
        }
        foreach (MeshRenderer mesh in meshes) {
            foreach (Material mat in mesh.materials) originalColor = mat.color;
        }
        foreach (SkinnedMeshRenderer mesh in skin) {
            foreach (Material mat in mesh.materials) originalColor = mat.color;
        }
        hit = false;
        if (list.Count>0)
        {
            tempList.Clear();
            for (int i = 0; i < generalData.dialogueQuestions.Length; i++)
            {
                if ((generalData.dialogueQuestions[i].typing == TypeQuestion.Wheelchair || generalData.dialogueQuestions[i].typing == TypeQuestion.Elderly)
                    && LevelManager.levelNum == LevelManager.LevelNum.Level1)
                {
                    tempList.Add(i);
                }//search for valid question with the correct type for level 1
                else if ((generalData.dialogueQuestions[i].typing == TypeQuestion.hearing || generalData.dialogueQuestions[i].typing == TypeQuestion.Visual)
                    && LevelManager.levelNum == LevelManager.LevelNum.Level2)
                {
                    tempList.Add(i);
                }//search for valid question with the correct type for level 2
                else if ((generalData.dialogueQuestions[i].typing == TypeQuestion.invisible || generalData.dialogueQuestions[i].typing == TypeQuestion.Autism
                    || generalData.dialogueQuestions[i].typing == TypeQuestion.intelluctual)
                   && LevelManager.levelNum == LevelManager.LevelNum.Level3)
                {
                    tempList.Add(i);
                }//search for valid question with the correct type for level 3
                return;
            }
        }
        for (int i = 0; i < generalData.dialogueQuestions.Length; i++)
        {
            if ((generalData.dialogueQuestions[i].typing == TypeQuestion.Wheelchair || generalData.dialogueQuestions[i].typing == TypeQuestion.Elderly)
                && LevelManager.levelNum == LevelManager.LevelNum.Level1) 
            {
                list.Add(i);
            }//search for valid question with the correct type for level 1
            else if((generalData.dialogueQuestions[i].typing == TypeQuestion.hearing || generalData.dialogueQuestions[i].typing == TypeQuestion.Visual)
                && LevelManager.levelNum == LevelManager.LevelNum.Level2)
            {
                list.Add(i);
            }//search for valid question with the correct type for level 2
            else if ((generalData.dialogueQuestions[i].typing == TypeQuestion.invisible || generalData.dialogueQuestions[i].typing == TypeQuestion.Autism
                || generalData.dialogueQuestions[i].typing == TypeQuestion.intelluctual)
               && LevelManager.levelNum == LevelManager.LevelNum.Level3)
            {
                list.Add(i);
            }//search for valid question with the correct type for level 3
        }
        tempList= new List<int>(list);
        for(int i = 0; i < 100; i++)
        {
            print("removed index: " + RandomIndexForGeneralQuestion());
            print("count: " + list.Count);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (tutorial.stop) return;
        Movement();
        if (invincibility) {
            invincibilitytime -= Time.deltaTime;
            float timer = 10f;

            for (int i = 0; i < meshes.Length; i++) {
                meshes[i].material.Lerp(originalMats[i], invincibleMats[i], Mathf.PingPong(Time.time * timer, 1));
            }
            skin[0].material.Lerp(originalMats[^1], invincibleMats[^1], Mathf.PingPong(Time.time * timer, 1));
        }
        if(invincibilitytime <= 0) {
            invincibility = false;
            invincibilitytime = originalInvincibleTime;
            Physics.IgnoreLayerCollision(gameObject.layer, 8, false); // obstacle layer
            skin[0].material = originalMats[^1];
            for (int i = 0; i < meshes.Length; i++) {
                meshes[i].material = originalMats[i];
            }
        }
        levelManager.energyslider.value = energy;
        if (canMove) {
            movespeed += .1f * Time.deltaTime;
            if (movespeed >= maxspeed) movespeed = maxspeed;
            transform.Translate(movespeed * Time.deltaTime * movement, Space.Self);
            if (movement.z > 0 && !hit) {
                levelManager.score++;
                if (!invincibility) {
                    energy -= .001f;
                }
            }
        }
        if (energy <= 0) {
            gameObject.SetActive(false);
            levelManager.gameover = true;
        }
        if (energy > maxenergy) energy = maxenergy;
        if (hit) {
            float timer = 10f;
            for (int i = 0; i < meshes.Length; i++) {
                meshes[i].material.color = Color.Lerp(originalColor, hitColor, Mathf.PingPong(Time.time * timer, 1));
            }
            skin[0].material.color = Color.Lerp(originalColor, hitColor, Mathf.PingPong(Time.time * timer, 1));
            hitTime -= Time.deltaTime;
        }
        if(hitTime <= 0) {
            hit = false;
            hitTime = 1.5f;
            rb.velocity = Vector3.zero;
            for (int i = 0; i < meshes.Length; i++) {
                meshes[i].material.color = originalColor;
            }
            skin[0].material.color = originalColor;
        }
    }
    public void Movement()
    {
        anim.SetBool("CanMove", canMove);
        anim.SetBool("Wheelchair", pushingWheelchair);
        if (!canMove) return;
        if (levelManager.level == LevelManager.Level.BusInterior && !levelManager.pausing) {
            movement = new Vector3(0, 0, (Input.GetButton("Fire1") || Input.GetAxisRaw("Vertical") > 0) ? 1 : (Input.GetButton("Fire2") || Input.GetAxisRaw("Vertical") < 0) ? -1 : 0);
        } 
        else {
            movement = new Vector3(0, 0, hit? .5f : 1);
        }
        if((Input.GetButton("Fire1") || Input.GetButton("Fire2")) && levelManager.level == LevelManager.Level.BusInterior)
        {
            anim.SetBool("CanMove", true);
        } 
        else if(movement.z == 0 && levelManager.level == LevelManager.Level.BusInterior)
        {
            anim.SetBool("CanMove", false);
        }
        else
        {
            anim.SetBool("CanMove", canMove);
        }
        RaycastHit tilehit; // for detecting tile to access lane variables
        Physics.Raycast(transform.position, Vector3.down, out tilehit);
        
        if (tilehit.collider)
        {
            print(tilehit.collider.GetComponent<Tile>());
            if (tilehit.collider.GetComponent<Tile>())
            {
                tile = tilehit.collider.GetComponent<Tile>();
            }
        }
        if(levelManager.level != LevelManager.Level.BusInterior && !levelManager.pausing) {
            RaycastHit left, right;
            Physics.Raycast(righthit.position, transform.right, out right, .7f);
            Debug.DrawRay(righthit.position, transform.right * .7f,Color.blue);
            Physics.Raycast(lefthit.position, -transform.right, out left, .7f);
            Debug.DrawRay(lefthit.position, -transform.right * .7f,Color.red);
            if (Input.GetButtonDown("Fire1") & animating == false & lane != 0 & left.collider == null) {
                newlane = Mathf.Clamp(lane - 1, 0, 2);
                StartCoroutine(LaneMoving());
            } 
            else if (Input.GetButtonDown("Fire2") & animating == false & lane !=2 & right.collider == null) {
                newlane = Mathf.Clamp(lane + 1, 0, 2);
                StartCoroutine(LaneMoving());
            }
        }
    }

    IEnumerator LaneMoving()
    {
        animating = true;
        float elapsedTime = 0;
        float duration = 0.2f;
        print("move");
        while (elapsedTime<duration)
        {

            float t = elapsedTime / duration;
            float lerpValue = Mathf.Lerp(tile.lanes[lane].z, tile.lanes[newlane].z, t);
            transform.position = new Vector3(transform.position.x,transform.position.y,lerpValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lane = newlane;
        transform.position = new Vector3(transform.position.x, transform.position.y, tile.lanes[newlane].z); ;
        animating = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(!canMove || NPC) return;
        if (invincibility) return;
        if (other.gameObject.layer == 8)
        {
            hit = true;
            if (other.gameObject.CompareTag("Knock")) {
                rb.AddForce(-transform.forward * 3, ForceMode.Impulse);
            }
            else other.collider.isTrigger = true;
            AudioManager.instance.PlaySFX(hitsfx);
            energy -= 10f;
        }
    }
    public void Invincibility()
    {
        invincibility = true;
        invincibilitytime = originalInvincibleTime;
        Physics.IgnoreLayerCollision(gameObject.layer, 8, true);
        for(int i = 0; i < meshes.Length; i++) {
            meshes[i].material = invincibleMats[i];
        }
        skin[0].material = invincibleMats[^1];
    }
    public int RandomIndexForGeneralQuestion()
    {
        if (list.Count <= 0)
        {
            list = new List<int>(tempList);
        }
        int chosen = Random.Range(0, list.Count);
        int index = list[chosen];
        list.RemoveAt(chosen);
        return index;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Interactable>()) interactable = true;
    }
    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<Interactable>()) interactable = false;
    }
}
[System.Serializable]
public enum TypeQuestion { General,Wheelchair, Visual,hearing,Elderly,Autism,invisible,intelluctual };