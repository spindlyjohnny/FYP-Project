using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector3 movement;
    public float movespeed;
    public bool canMove;
    LevelManager levelManager;
    //NPCManagement npcmanager;
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        levelManager = FindObjectOfType<LevelManager>();
        //npcmanager = FindObjectOfType<NPCManagement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (canMove)transform.Translate(movespeed * Time.deltaTime * movement);
        //Quaternion toRotation = Quaternion.LookRotation(movement.normalized, Vector3.up);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 12 * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<Vehicle>()) {
            gameObject.SetActive(false);
            levelManager.gameover = true;
        }
    }
}
