using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    Vector3 movement;
    public float movespeed, maxspeed;
    public bool canMove, NPC;
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
    Tile tile;
    public Image avatar;
    public Sprite dialogueSprite, loseSprite, pauseSprite; // these vars exist cuz there's 2 playable charas
    public Animator anim;
    public int lane = 1, newlane;
    public Transform npcPosition;
    bool animating = false;
    public Material[] invincibleMats;
    public Material[] originalMats;
    bool hit;
    public GeneralDIalogueSO generalData;
    public static List<int> list= new List<int>();
    public List<int> tempList= new List<int>();
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
        canMove = true;
        levelManager = FindObjectOfType<LevelManager>();
        invincibilitytime = originalInvincibleTime;
        avatar.sprite = dialogueSprite;
        levelManager.energyslider.maxValue = maxenergy;
        //npcmanager = FindObjectOfType<NPCManagement>();
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

    }
    // Update is called once per frame
    void Update()
    {
        if (tutorial.stop) return;
        Movement();
        if (invincibility) {
            invincibilitytime -= Time.deltaTime;
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
            
            
            
            if (movement.z > 0 /*&& !Physics.Raycast(transform.position, transform.forward, .1f)*/) {
                levelManager.score++;
                if(!invincibility)energy -= .01f;
            }
        }
        if(energy <= 0) {
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
            //GetComponent<Rigidbody>().velocity = Vector3.zero;
            //canMove = false;
            //GetComponent<Rigidbody>().AddForce(-transform.forward * 3f, ForceMode.Force);
            //GetComponent<Rigidbody>().isKinematic = true;
            //GetComponent<Collider>().isTrigger = true;
            //Physics.IgnoreLayerCollision(gameObject.layer, 8, true);
            hitTime -= Time.deltaTime;
        }
        if(hitTime <= 0) {
            hit = false;
            hitTime = 1.5f;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            //GetComponent<Rigidbody>().isKinematic = false;
            //Physics.IgnoreLayerCollision(gameObject.layer, 8, false);
            //GetComponent<Collider>().isTrigger = false;
            for (int i = 0; i < meshes.Length; i++) {
                meshes[i].material.color = originalColor;
            }
            skin[0].material.color = originalColor;
            //canMove = true;
        }
    }
    //IEnumerator TempInvincible() {
    //    yield return new WaitForSeconds(.2f);
    //    canMove = true;
    //}
    public void Movement()
    {
        anim.SetBool("CanMove", canMove);
        if (!canMove) return;
        if (levelManager.level == LevelManager.Level.BusInterior && !levelManager.pausing) {
            movement = new Vector3(0, 0, Input.GetButton("Fire1") ? 1 : Input.GetButton("Fire2") ? -1 : 0);
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
        Physics.Raycast(transform.position, Vector3.down, out tilehit);if(!invincibility)energy -= .01f;
        if (tilehit.collider)
        {
            if (tilehit.collider.GetComponent<Tile>())
            {
                tile = tilehit.collider.GetComponent<Tile>();
            }
        }
        if(levelManager.level != LevelManager.Level.BusInterior && !levelManager.pausing) {
            RaycastHit left, right;
            Physics.Raycast(transform.position, transform.right, out right, .67f);
            Physics.Raycast(transform.position, -transform.right, out left, .67f);
            if (left.collider != null || right.collider != null) return;
            if (Input.GetButtonDown("Fire1") & animating == false) {
                newlane = Mathf.Clamp(lane - 1, 0, 2);
                StartCoroutine(LaneMoving());
            } 
            else if (Input.GetButtonDown("Fire2") & animating == false) {
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
                GetComponent<Rigidbody>().AddForce(-transform.forward * 3, ForceMode.Impulse);
            }
            else other.collider.isTrigger = true;
            AudioManager.instance.PlaySFX(hitsfx);
            energy -= energy * .1f;
        }
    }
    public void Invincibility()
    {
        invincibility = true;
        invincibilitytime = originalInvincibleTime;
        Physics.IgnoreLayerCollision(gameObject.layer, 8, true);
        //Physics.IgnoreLayerCollision(gameObject.layer, 7, true);
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
    //IEnumerator HitReaction()
    //{
    //    float timer = 10f;
    //    for (int i = 0; i < meshes.Length; i++) {
    //        meshes[i].material.color = Color.Lerp(originalColor, hitColor, Mathf.PingPong(Time.time * timer,1));
    //    }
    //    skin[0].material.color = Color.Lerp(originalColor, hitColor, Mathf.PingPong(Time.time * timer,1));
    //    AudioManager.instance.PlaySFX(hitsfx);
    //    canMove = false;
    //    GetComponent<Rigidbody>().AddForce(-transform.forward * 3, ForceMode.Impulse);
    //    yield return new WaitForSeconds(0.5f);
    //    GetComponent<Rigidbody>().velocity = Vector3.zero;
    //    for (int i = 0; i < meshes.Length; i++) {
    //        meshes[i].material.color = originalColor;
    //    }
    //    skin[0].material.color = originalColor;
    //    canMove = true;
    //}
}
[System.Serializable]
public enum TypeQuestion { General,Wheelchair, Visual,hearing,Elderly,Autism,invisible,intelluctual };