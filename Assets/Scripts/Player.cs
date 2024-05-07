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
    //NPCManagement npcmanager;
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        levelManager = FindObjectOfType<LevelManager>();
        energy = maxenergy;
        levelManager.energyslider.maxValue = maxenergy;
        //npcmanager = FindObjectOfType<NPCManagement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        levelManager.energyslider.value = energy;
        if (canMove) {
            transform.Translate(movespeed * Time.deltaTime * movement, Space.Self);
            if (movement != Vector3.zero) {
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
            energy -= 10;
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Collectible>()) {
            energy += 10;
            if (energy > maxenergy) energy = maxenergy;
        }
    }
}
