using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ObstacleSpawn myspawner;
    public Vector3 spawnoffset;
    public Vector3 dir;
    float oppositeLength=0.6f;
    int laneIndex;
    float sensorLength =5f;
    protected LevelManager levelManager;
    //Player player; 
    public Transform front, right, left;
    public float[] lanes = {-1.3f,0,1.3f };
    //Vector3 inital;
    Rigidbody rb;
    //float timeElapsed;
    public float elapsedFromMoved=0;
    public bool animating=false;
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
        Transform[] rays = { front, front, front };
        RaycastHit hit;
        LaneProperty[] lane= new LaneProperty[3];
        float distanceBetweenLane = 1.3f;
        //index 2 is left, index 1 is middle, index 0 is right
        if (Physics.Raycast(new Vector3(rays[0].position.x, rays[0].position.y, -distanceBetweenLane), rays[0].forward, out hit, sensorLength, LayerMask.GetMask("NPC Obstacle", "Player", "NPC")))
        {//this is right
            Debug.DrawLine(rays[0].position, hit.point, Color.gray);
            lane[0].isAvaliable = false;
        }
        else
        {
            lane[0].isAvaliable = true;

        }

        if (Physics.Raycast(new Vector3(rays[0].position.x, rays[0].position.y,0), rays[0].forward, out hit, sensorLength, LayerMask.GetMask("NPC Obstacle", "Player", "NPC")))
        {//this is middle
            Debug.DrawLine(rays[0].position, hit.point, Color.gray);
            lane[1].isAvaliable = false;
        }
        else
        {
            lane[1].isAvaliable = true;
        }

        if (Physics.Raycast(new Vector3(rays[0].position.x, rays[0].position.y, distanceBetweenLane), rays[0].forward, out hit, sensorLength, LayerMask.GetMask("NPC Obstacle", "Player", "NPC")))
        {//this is left
            Debug.DrawLine(rays[0].position, hit.point, Color.gray);
            lane[2].isAvaliable = false;
        }
        else
        {
            lane[2].isAvaliable = true;
        }



        if (transform.position.z > distanceBetweenLane / 2)//check whether the obstacle is in the left lane
        {
            laneIndex = 2;
        }else if(transform.position.z < distanceBetweenLane / 2|| transform.position.z > -distanceBetweenLane / 2)//check whether the obstacle is in the middle lane
        {
            laneIndex = 1;
        }
        else if (transform.position.z <-distanceBetweenLane/2)//check whether the obstacle is in the right lane
        {
            laneIndex = 0;
        }
        List<int> avaliability = new List<int>();
        for (int i = 0; i < lane.Length; i++)
        {
            if (lane[i].isAvaliable == true)//this loop checks which lane is available
            {
                avaliability.Add(i);//add the index to list
            }
        }
        int random;
        if (avaliability.Count == 0)//if there is no available lane, it check for whether which obstacle in the lane is the furthest away
        {
            int temp = 0;
            for (int i = 1; i < lane.Length; i++)
            {
                if (lane[i - 1].distance > lane[i].distance) continue;//if the previous lane's distance is more than the current lane distance then just iterate
                temp = i;
            }
            random = temp;
            if (random == laneIndex) return;//if the obstacle that is furthest away in the same lane, then don't move from the lane
        }
        else
        {
            random = avaliability[Random.Range(0, avaliability.Count)];
        }
        if (lane[laneIndex].isAvaliable == true & animating==false)
        {
            animating = true;
            StartCoroutine(LaneMoving(random));

        }
        /*
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
        }*/

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
    IEnumerator LaneMoving(int random)
    {
        
        animating = true;
        float elapsedTime = 0;
        float duration = 0.4f;
        int index = laneIndex;

        while (elapsedTime < duration)
        {

            float t = elapsedTime / duration;
            float lerpValue = Mathf.Lerp(lanes[index], lanes[random] , t);
            transform.position = new Vector3(transform.position.x, transform.position.y, lerpValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, lanes[random]); ;
        animating = false;
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
    public class LaneProperty
    {
        public bool isAvaliable;
        public int distance;
    }
}

