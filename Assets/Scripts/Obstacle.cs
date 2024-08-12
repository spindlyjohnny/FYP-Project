using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ObstacleSpawn myspawner;
    public Vector3 spawnoffset;
    public Vector3 dir;
    float oppositeLength=0.6f;
    public float sensorLength =5f;
    protected LevelManager levelManager;
    //Player player; 
    public Transform front, right, left;
    //Vector3 inital;
    Rigidbody rb;
    //float timeElapsed;
    public float elapsedFromMoved=0;
    int laneIndex;
    //float lerpDuration02 = 1f;
    //float duration = 0.8f;
    //bool lerp = false;
    //float valueToLerp;
    //bool hitPlayer=false;
    //[SerializeField] Collider trigger;
    //[SerializeField] int hits = 0;
    //public int lane = 1, newlane;
    //Tile tile;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        rb = GetComponent<Rigidbody>();
        //oppositeLength = FindObjectOfType<Tile>().lanes[0].z;
        //inital = transform.position;
        //print(inital.z);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        rb.velocity = (5 * dir);
        //Sensors();
        //if (!myspawner.gameObject.activeSelf) Destroy(gameObject);
        
        
    }
    //private void OnTriggerEnter(Collider other) {
    //    //RaycastHit hit;
    //    if (other) {
    //        var direction = other.transform.position - transform.position;
    //        if (Physics.Raycast(transform.position, direction, 1f, LayerMask.GetMask("NPC Obstacle", "Player"/*,"Obstacle"*/, "NPC"))) {
    //            dir = transform.position - other.transform.position;
    //        }

    //    }
    //}
    //private void OnTriggerExit(Collider other) {
    //    //List<string> layers = new List<string> { "NPC Obstacle","Player","NPC"};
    //    if (other/*&& layers.Contains(other.gameObject.layer.ToString())*/) {
    //        Sensors();
    //    }
    //}
    //void Sensors() {
    //    Transform[] rays = { front, left, right };
    //    RaycastHit hit;

    //    // First, check the forward direction
    //    if (!Physics.Raycast(front.position, front.forward, out hit, 1f, LayerMask.GetMask("NPC Obstacle", "Player", "NPC"))) {
    //        // If forward is clear, move in that direction
    //        dir = front.forward;
    //        return;
    //    }

    //    // If forward is blocked, check other directions
    //    for (int i = 0; i < rays.Length; i++) {
    //        if (!Physics.Raycast(rays[i].position, rays[i].forward, out hit, 1f, LayerMask.GetMask("NPC Obstacle", "Player", "NPC"))) {
    //            // Move in the direction of the first clear path
    //            dir = rays[i].forward;
    //            return;
    //        }
    //    }

    //    // If all directions are blocked, move backwards
    //    dir = -front.forward;
    //}
    void Sensors() {
        Transform[] rays = { front, front, front };//going right increase the y axis rotation value
        RaycastHit hit;
        bool[] direction = { false, false, false };
        float angleInRadian = Mathf.Tan(oppositeLength / sensorLength);
        float lengthOfSideRay =sensorLength / Mathf.Cos(angleInRadian);
        if(Physics.Raycast(rays[0].position, rays[0].forward, out hit, sensorLength, LayerMask.GetMask("NPC Obstacle", "Player", "NPC")))
        {//this is middle
            Debug.DrawLine(rays[0].position, hit.point, Color.gray);
            direction[1] = true;
        }
        else
        {
            direction[1] = false;
        }

        if(Physics.Raycast(rays[1].position, rays[1].forward+new Vector3(0,0, angleInRadian), out hit, lengthOfSideRay, LayerMask.GetMask("NPC Obstacle", "Player", "NPC")))
        {//this is right
            Debug.DrawLine(rays[0].position,hit.point, Color.green);
            direction[0] = true;
        }
        else
        {
            direction[0] = false;
        }
        if (Physics.Raycast(rays[1].position, rays[1].forward - new Vector3(0, 0, angleInRadian), out hit, lengthOfSideRay, LayerMask.GetMask("NPC Obstacle", "Player", "NPC")))
        {//this is right
            Debug.DrawLine(rays[0].position, hit.point, Color.green);
            direction[2] = true;
        }
        else
        {
            direction[2] = false;
        }

        int hits = 0;
        /*
        Vector3 avoidDir = Vector3.zero;

        for (int i = 0; i < rays.Length; i++) {
            bool isHit = Physics.Raycast(rays[i].position, rays[i].forward, out hit, 1f, LayerMask.GetMask("NPC Obstacle", "Player", "NPC"));

            if (isHit) {
                print(hit.collider.name);
                Debug.DrawLine(rays[i].position, hit.point, Color.red);
                hits++;
                avoidDir += (rays[i].position - hit.point).normalized; // Accumulate avoidance direction
            } else {
                Debug.DrawRay(rays[i].position, rays[i].forward * 1f, Color.green);
            }
        }

        if (hits > 0 && hits < 3) {
            // Avoid the detected obstacles
            print("Avoiding obstacles");
            dir = (avoidDir / hits).normalized; // Average avoidance direction
        } else if (hits >= 3) {
            print("Back");
            dir = -front.forward;
        } else {
            dir = front.forward;
        }

        print("Total hits: " + hits);*/
    }

    //void Sensors() {
    //    Transform[] rays = { front, left, right };
    //    RaycastHit hit;
    //    for (int i = 0; i < rays.Length; i++) {
    //        Physics.Raycast(rays[i].position, rays[i].forward, out hit, 1f, LayerMask.GetMask("NPC Obstacle", "Player"/*,"Obstacle"*/, "NPC"));
    //        if (hit.collider == null) {
    //            print("hit");
    //            dir = rays[i].forward;//transform.position - hit.collider.transform.position;
    //        }
    //    }
    //}
}
