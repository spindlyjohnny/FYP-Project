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
    public float energygain;
    public MeshRenderer[] meshes;
    public Color originalColor;
    public AudioClip hitsfx;
    public GameObject inputtext;
    public bool invincibility = false;
    //NPCManagement npcmanager;
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        levelManager = FindObjectOfType<LevelManager>();
        energy = maxenergy;
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
        if(Input.GetAxisRaw("Horizontal") !=0 || Input.GetAxisRaw("Vertical") != 0)
        {
            movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        }else if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            movement = new Vector3(0, 0, 0);
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

    public void Invincible()
    {
        StartCoroutine(Invincibility());
    }

    public IEnumerator Invincibility()
    {
        invincibility = true;
        GetComponent<Rigidbody>().isKinematic = true;
        for (int i =0; i< 5; i++)
        {
            foreach (MeshRenderer mesh in meshes)
            {
                foreach (Material mat in mesh.materials) mat.color = Color.blue;
            }
            yield return new WaitForSeconds(10f);
            foreach (MeshRenderer mesh in meshes)
            {
                foreach (Material mat in mesh.materials) mat.color = originalColor;
            }
            yield return new WaitForSeconds(0.5f);
            GetComponent<Rigidbody>().isKinematic = false;
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
