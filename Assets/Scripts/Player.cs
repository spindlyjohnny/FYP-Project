using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    Vector3 movement;
    public float movespeed;
    public bool canMove,NPC;
    LevelManager levelManager;
    public float energy,maxenergy;
    //public float energygain,maxEnergyGain;
    public MeshRenderer[] meshes;
    public SkinnedMeshRenderer[] skin;
    public Color originalColor,hitColor;
    public AudioClip hitsfx;
    public GameObject inputtext;
    public bool invincibility = false;
    public float originalInvincibleTime,maxInvincibleTime;
    float direction=3;
    [SerializeField]float invincibilitytime;
    Tile tile;
    public Image avatar;
    public Sprite dialogueSprite; // this var exists cuz there's 2 playable charas
    public Animator anim;
    public int lane=1, newlane;
    Vector3 startPos;
    [HideInInspector]public Vector3 distTravelled;
    //NPCManagement npcmanager;
    bool animating = false;
    //Rigidbody rb;
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
        foreach (MeshRenderer mesh in meshes) {
            foreach(Material mat in mesh.materials)originalColor=mat.color;
        }
        foreach (SkinnedMeshRenderer mesh in skin)
        {
            foreach (Material mat in mesh.materials) originalColor = mat.color;
        }
        startPos = transform.position;
        //rb = GetComponent<Rigidbody>();
        //if(levelManager.level == LevelManager.Level.Bus) {
        //    GetComponent<Collider>().isTrigger = true;
        //} 
        //else {
        //    GetComponent<Collider>().isTrigger = false;
        //}
        //Physics.IgnoreLayerCollision(gameObject.layer, 8, false);
        //Physics.IgnoreLayerCollision(gameObject.layer, 7, false);
    }
    // Update is called once per frame
    void Update()
    {
        Movement();
        distTravelled = transform.position - startPos;
        //print("Distance"+Mathf.FloorToInt(distTravelled.magnitude));
        //print("Time:"+Time.timeSinceLevelLoad);
        if (invincibility) {
            invincibilitytime -= Time.deltaTime;
        }
        if(invincibilitytime <= 0) {
            invincibility = false;
            invincibilitytime = originalInvincibleTime;
            //movespeed /= 2;
            //trailRenderer.emitting = false;
            //GetComponent<Rigidbody>().isKinematic = false;
            Physics.IgnoreLayerCollision(gameObject.layer, 8, false); // obstacle layer
            Physics.IgnoreLayerCollision(gameObject.layer, 7, false); // npc layer
            for (int i = 0; i < 5; i++) {
                foreach (MeshRenderer mesh in meshes) {
                    foreach (Material mat in mesh.materials) mat.color = originalColor;
                }
                foreach (SkinnedMeshRenderer mesh in skin)
                {
                    foreach (Material mat in mesh.materials) mat.color = originalColor;
                }
            }
        }
        levelManager.energyslider.value = energy;
        if (canMove) {
            movespeed += .1f * Time.deltaTime;
            transform.Translate(movespeed * Time.deltaTime * movement, Space.Self);
            if (movement.z > 0) {
                levelManager.score++;
                if(!invincibility)energy -= .01f;
            }
        }
        if(energy <= 0) {
            gameObject.SetActive(false);
            levelManager.gameover = true;
        }
        if (energy > maxenergy) energy = maxenergy;
    }

    void Movement()
    {
        if (!canMove) return;
        movement = new Vector3(0, 0,levelManager.level == LevelManager.Level.BusInterior ? Input.GetAxisRaw("Vertical") : 1);      
        if((Input.GetAxisRaw("Vertical")==1 || Input.GetAxisRaw("Vertical") == -1)&& levelManager.level == LevelManager.Level.BusInterior)
        {
            anim.SetBool("CanMove", true);
        } else if(Input.GetAxisRaw("Vertical") == 0&& levelManager.level == LevelManager.Level.BusInterior)
        {
            anim.SetBool("CanMove", false);
        }
        else
        {
            anim.SetBool("CanMove", canMove);
        }
        RaycastHit hit; // for detecting tile to access lane variables
        Physics.Raycast(transform.position, Vector3.down, out hit);
        if (hit.collider)
        {
            if (hit.collider.GetComponent<Tile>())
            {
                tile = hit.collider.GetComponent<Tile>();
            }
        }
        if (Input.GetAxisRaw("Horizontal") < 0 && direction != Input.GetAxisRaw("Horizontal") & animating == false)
        { // need to figure out how to prevent this from happening when holding down button
          // taken from unreal endless runner lololol
            print("change direction");
            print(direction);
            newlane = Mathf.Clamp(lane - 1, 0, 2);
            /*
            Vector3 lerpPosition = new Vector3(transform.position.x, transform.position.y, tile.lanes[newlane].z);
            //print(lerpPosition);
            float distanceBetween = Vector3.Magnitude(transform.position - lerpPosition);
            while (distanceBetween > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, lerpPosition, movespeed * Time.deltaTime);
                distanceBetween = Vector3.Magnitude(transform.position - lerpPosition);
            }
            transform.position = lerpPosition;*/
            StartCoroutine(LaneMoving());
            //transform.position = tile.lanes[tile.newlane]\
        }
        else if (Input.GetAxisRaw("Horizontal") > 0 && direction != Input.GetAxisRaw("Horizontal") &animating==false)
        {
            print("yes");
            
            newlane = Mathf.Clamp(lane + 1, 0, 2);
            print(newlane);
            /*
            Vector3 lerpPosition = new Vector3(transform.position.x, transform.position.y, tile.lanes[newlane].z);
            //print(lerpPosition);
            float distanceBetween = Vector3.Magnitude(transform.position - lerpPosition);
            while (distanceBetween > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, lerpPosition, movespeed * Time.deltaTime);
                distanceBetween = Vector3.Magnitude(transform.position - lerpPosition);
            }
            transform.position = lerpPosition;*/
            StartCoroutine(LaneMoving());
            
        }
        direction = Input.GetAxisRaw("Horizontal");
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
        if(NPC) return;
        if (invincibility) return;
        if (other.gameObject.layer == 8)
        {
            print("yes");
            StartCoroutine(HitReaction());
            energy -= 10;
        }
    }
    public void Invincibility()
    {
        invincibility = true;
        invincibilitytime = originalInvincibleTime;
        Physics.IgnoreLayerCollision(gameObject.layer, 8, true);
        Physics.IgnoreLayerCollision(gameObject.layer, 7, true);
        //GetComponent<Rigidbody>().isKinematic = true;
        for (int i = 0; i < 5; i++) {
            foreach (MeshRenderer mesh in meshes) {
                foreach (Material mat in mesh.materials) mat.color = Color.red;
            }
        }
        foreach (SkinnedMeshRenderer mesh in skin) {
            foreach (Material mat in mesh.materials) mat.color = Color.red;
        }
        //trailRenderer.emitting = true;
        //movespeed *= 2;

    }

    IEnumerator HitReaction()
    {
        foreach (MeshRenderer mesh in meshes)
        {
            foreach (Material mat in mesh.materials) mat.color = hitColor;
        }
        foreach (SkinnedMeshRenderer mesh in skin)
        {
            foreach (Material mat in mesh.materials) mat.color = Color.red;
        }
        AudioManager.instance.PlaySFX(hitsfx);
        canMove = false;
        yield return new WaitForSeconds(0.5f);
        foreach (MeshRenderer mesh in meshes)
        {
            foreach (Material mat in mesh.materials) mat.color = originalColor;
        }
        foreach (SkinnedMeshRenderer mesh in skin)
        {
            foreach (Material mat in mesh.materials) mat.color = originalColor;
        }
        canMove = true;
    }

    //private void OnTriggerEnter(Collider other) {
    //    if (other.GetComponent<Collectible>()) {
    //        if (other.CompareTag("Energy")) {
    //            energy += energygain;
    //            if (energy > maxenergy) energy = maxenergy;
    //        } 
    //        else {
    //            // question.
    //        }
    //    }
    //}
}
