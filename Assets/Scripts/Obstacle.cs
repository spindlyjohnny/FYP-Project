using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ObstacleSpawn myspawner;
    public Vector3 spawnoffset;
    public Vector3 dir;
    protected LevelManager levelManager;
    Player player; 
    public Transform front, right, left;
    Vector3 inital;
    Rigidbody rb;
    float timeElapsed;
    public float elapsedFromMoved=0;
    float lerpDuration02 = 1f;
    float duration = 0.8f;
    bool lerp = false;
    float valueToLerp;
    bool hitPlayer=false;
    //[SerializeField] int hits = 0;
    //public int lane = 1, newlane;
    //Tile tile;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        rb = GetComponent<Rigidbody>();
        //inital = transform.position;
        //print(inital.z);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        rb.velocity = (5 * dir);
        //if(dir == Vector3.zero)
        //{
        //    dir = -Vector3.right;//-1 in the x-axis is going forward
        //}
        //Sensors();//this is the sensor
        //Feelers();
        Sensors();
        //if (!myspawner.gameObject.activeSelf) Destroy(gameObject);
        
        
    }
    //void Sensors() {
    //    Transform[] rays = { front, left, right };
    //    RaycastHit hit;
    //    int hits = 0;
    //    for (int i = 0; i < rays.Length; i++) {
    //        Physics.Raycast(rays[i].position, rays[i].forward, out hit, 1f, LayerMask.GetMask("NPC Obstacle", "Player"/*,"Obstacle"*/, "NPC"));
    //        if (hit.collider) {
    //            print(hit.collider.name);
    //            Debug.DrawLine(rays[i].position, hit.point, Color.red);
    //            dir = transform.position - hit.collider.transform.position;
    //            hits++;
    //            print(hits);
    //        }
    //        print(hit.collider.name);
    //        if (hit.collider == null && hits > 0 && hits < 3/*&& (transform.position - hit.collider.transform.position).sqrMagnitude < .2f*/) {
    //            runs when 1 or 2 objects are detected
    //            print(rays[i]);
    //            dir = rays[i].forward;
    //        } else if (hits >= 3) {
    //            print("back");
    //            hits = 0;
    //            dir = -front.forward;
    //        } else if (hits == 0) {
    //            dir = front.forward;
    //        } else {
    //            dir = new Vector3(-1, 0, 0);
    //        }
    //    }
    //}
    //void Sensors() {
    //    Transform[] rays = { front, left, right };
    //    RaycastHit hit;
    //    int hits = 0;
    //    Vector3 avoidDir = Vector3.zero;

    //    for (int i = 0; i < rays.Length; i++) {
    //        if (Physics.Raycast(rays[i].position, rays[i].forward, out hit, 1f, LayerMask.GetMask("NPC Obstacle", "Player", "NPC"))) {
    //            print(hit.collider.name);
    //            Debug.DrawLine(rays[i].position, hit.point, Color.red);
    //            hits++;
    //            avoidDir += -rays[i].forward; // Accumulate avoidance direction
    //        } else {
    //            Debug.DrawRay(rays[i].position, rays[i].forward * 1f, Color.green);
    //        }
    //    }

    //    print("Total hits: " + hits);

    //    if (hits > 0) {
    //        if (hits >= 3) {
    //            print("back");
    //            dir = -front.forward;
    //        } else {
    //            print("avoid");
    //            dir = (avoidDir / hits).normalized; // Average avoidance direction
    //        }
    //    } else {
    //        dir = front.forward;
    //    }
    //}
    void Sensors() {
        Transform[] rays = { front, left, right };
        RaycastHit hit;
        int hits = 0;
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

        print("Total hits: " + hits);
    }

}
