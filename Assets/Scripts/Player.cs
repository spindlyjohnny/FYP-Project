using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    Vector3 movement;
    public float movespeed;
    public bool canMove,NPC;
    LevelManager levelManager;
    public float energy,maxenergy;
    public float energygain,maxEnergyGain;
    public MeshRenderer[] meshes;
    public Color originalColor;
    public AudioClip hitsfx;
    public GameObject inputtext;
    public bool invincibility = false;
    public float originalInvincibleTime,maxInvincibleTime;
    [SerializeField]float invincibilitytime;
    Tile tile;
    //NPCManagement npcmanager;
    // Start is called before the first frame update
    private void Awake() {
        if (PlayerPrefs.GetInt("bool") == 1) {
            energy = PlayerPrefs.GetFloat("energy");
            energygain = PlayerPrefs.GetFloat("Energy Gain");
            originalInvincibleTime = PlayerPrefs.GetFloat("Invincibility Time");
        } else {
            PlayerPrefs.SetFloat("energy", 100);
            PlayerPrefs.SetFloat("Energy Gain", 10f);
            PlayerPrefs.SetFloat("Invincibility Time", 10f);
            energy = PlayerPrefs.GetFloat("energy");
        }
    }
    void Start()
    {
        canMove = true;
        levelManager = FindObjectOfType<LevelManager>();
        invincibilitytime = originalInvincibleTime;
        //energy = maxenergy;
        levelManager.energyslider.maxValue = maxenergy;
        //npcmanager = FindObjectOfType<NPCManagement>();
        meshes = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh in meshes) {
            foreach(Material mat in mesh.materials)originalColor=mat.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 1);
        RaycastHit hit; // for detecting tile to access lane variables
        Physics.Raycast(transform.position, Vector3.down, out hit);
        if (hit.collider) {
            if (hit.collider.GetComponent<Tile>()) {
                tile = hit.collider.GetComponent<Tile>();
            }
        }
        if(Input.GetAxisRaw("Horizontal") < 0) { // need to figure out how to prevent this from happening when holding down button
            // taken from unreal endless runner lololol
            tile.newlane = Mathf.Clamp(tile.lane - 1, 0, 2);
            Vector3 lerpPosition = new Vector3(tile.lanes[tile.newlane].x, transform.position.y, tile.lanes[tile.newlane].z);
            transform.position = Vector3.Lerp(transform.position,lerpPosition, movespeed * Time.deltaTime);
            //transform.position = tile.lanes[tile.newlane];
        }
        else if(Input.GetAxisRaw("Horizontal") > 0) {
            tile.newlane = Mathf.Clamp(tile.lane + 1, 0, 2);
            Vector3 lerpPosition = new Vector3(tile.lanes[tile.newlane].x, transform.position.y, tile.lanes[tile.newlane].z);
            transform.position = Vector3.Lerp(transform.position,lerpPosition,movespeed * Time.deltaTime);
        }
        if (invincibility) {
            invincibilitytime -= Time.deltaTime;
        }
        if(invincibilitytime <= 0) {
            invincibility = false;
            invincibilitytime = originalInvincibleTime;
            GetComponent<Rigidbody>().isKinematic = false;
            for (int i = 0; i < 5; i++) {
                foreach (MeshRenderer mesh in meshes) {
                    foreach (Material mat in mesh.materials) mat.color = originalColor;
                }
            }
        }
        levelManager.energyslider.value = energy;
        if (canMove) {
            transform.Translate(movespeed * Time.deltaTime * movement, Space.Self);
            if (movement.z > 0) {
                levelManager.score++;
                energy -= .01f;
            }
        }
        if(energy <= 0) {
            gameObject.SetActive(false);
            levelManager.gameover = true;
        }

    }
    private void OnCollisionEnter(Collision collision) {
        if (NPC) return;
        if (invincibility) return;
        if (collision.gameObject.layer == 8) {
            StartCoroutine(HitReaction());
            energy -= 10;
        }
    }
    public void Invincibility()
    {
        invincibility = true;
        invincibilitytime = originalInvincibleTime;
        GetComponent<Rigidbody>().isKinematic = true;
        for (int i =0; i< 5; i++)
        {
            foreach (MeshRenderer mesh in meshes)
            {
                foreach (Material mat in mesh.materials) mat.color = Color.blue;
            }
        }
    }

    IEnumerator HitReaction()
    {
        foreach (MeshRenderer mesh in meshes)
        {
            foreach (Material mat in mesh.materials) mat.color = Color.red;
        }
        AudioManager.instance.PlaySFX(hitsfx);
        yield return new WaitForSeconds(0.5f);
        print("Change color");
        foreach (MeshRenderer mesh in meshes)
        {
            foreach (Material mat in mesh.materials) mat.color = originalColor;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Collectible>()) {
            if (other.CompareTag("Energy")) {
                energy += energygain;
                if (energy > maxenergy) energy = maxenergy;
            } 
            else {
                // question.
            }
        }
    }
}
