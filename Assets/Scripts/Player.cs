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
    List<Color> originalColor = new List<Color>(0);
    public AudioClip hitsfx;
    public GameObject inputtext;
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
            foreach(Material mat in mesh.materials)originalColor.Add(mat.color);
        }
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
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
        if (collision.gameObject.layer == 8) {
            StartCoroutine(HitReaction());
            energy -= 10;
        }
    }

    IEnumerator HitReaction()
    {
        foreach (MeshRenderer mesh in meshes) {
            foreach (Material mat in mesh.materials) mat.color = Color.red;
        }
        AudioManager.instance.PlaySFX(hitsfx);
        yield return new WaitForSeconds(0.5f);
        print("Change color");
        for (int i = 0; i < originalColor.Count; i++) {
            for(int x = 0; x < meshes.Length; x++) {
                meshes[x].materials[i].color = originalColor[i];
            }
            //foreach(MeshRenderer mesh in meshes) {
            //    mesh.materials[i].color = originalColor[i];
            //}
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
