using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ObstacleSpawn myspawner;
    public Vector3 spawnoffset;
    public Vector3 dir;
    protected LevelManager levelManager;
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
    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        rb = GetComponent<Rigidbody>();
        inital = transform.position;
        //print(inital.z);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        rb.velocity = (5 * dir);
        if(dir== new Vector3(0, 0, 0))
        {
            dir = new Vector3(-1, 0, 0);//-1 in the x-axis is going forward
        }
        Sensors();//this is the sensor
        if (!myspawner.gameObject.activeSelf) Destroy(gameObject);
        
        //else Destroy(gameObject, 15f);
    }
   void Sensors() {
        RaycastHit hit;
        Vector3 temp;
        bool fronthit = false, lefthit = false, righthit = false;
        
        // casts rays in 3 directions. if any ray detects an object, move away from object until there is nothing blocking.
        // current problem is that ai doesnt know what to do if more than 1 ray detects an object. unsure of how to do this efficiently.
        if(Physics.Raycast(front.position,front.forward,out hit, 1f)) {
            fronthit = true;
            float difference = 0.2f;
            if (Mathf.Abs(hit.collider.transform.position.z - inital.z) < difference & lerp == false)//if you are not lerping and the distance between the hit and and z is less than 0.2f
            {
                
                valueToLerp= difference - (hit.collider.transform.position.z - inital.z);
                lerp = true;
                transform.position = new Vector3(transform.position.x, transform.position.y,inital.z+ valueToLerp);

                temp = (transform.position - hit.transform.position );
            }
            else
            {
                temp = (transform.position - hit.transform.position);
            }            
            //honestly no idea how this works but the direction is the diference between the player and the object with -x and z
            if (temp.sqrMagnitude <= .5f) {
                
                dir = new Vector3(-temp.x,0,temp.z).normalized;
            }
            if (temp.sqrMagnitude > .5f) {//if the target is the more than the root of 0.5f away from the player just continue as norma;l 
                dir = -Vector3.right;
                fronthit = false;
            }
            Debug.DrawLine(front.position, hit.point,Color.red);
        }
        if(Physics.Raycast(right.position,right.forward,out hit, 1f)) {
            righthit = true;
            if (hit.collider.transform.position.z == transform.position.z)
            {
                temp = (transform.position - hit.transform.position );

            }
            else
            {
                temp = (transform.position - hit.transform.position);
            }
            if (temp.sqrMagnitude <= .5f)
            {//honestly no idea how this works but the direction is the diference between the player and the object with -x and z
                dir = new Vector3(-temp.x, 0, temp.z).normalized;
            }
            if (temp.sqrMagnitude > .5f)
            {//if the target is the more than the root of 0.5f away from the player just continue as norma;l 
                dir = -Vector3.right;
                righthit = false;
            }
            Debug.DrawLine(right.position, hit.point,Color.red);
        }
        if(Physics.Raycast(left.position,left.forward,out hit, 1f)) {
            lefthit = true;
            if (hit.collider.transform.position.z == transform.position.z)
            {
                temp = (transform.position - hit.transform.position );
            }
            else
            {
                temp = (transform.position - hit.transform.position);
            }
            if (temp.sqrMagnitude <= .5f)
            {//honestly no idea how this works but the direction is the diference between the player and the object with -x and z
                dir = new Vector3(-temp.x, 0, temp.z).normalized;
            }
            if (temp.sqrMagnitude > .5f)
            {//if the target is the more than the root of 0.5f away from the player just continue as norma;l 
                dir = -Vector3.right;
                lefthit = false;
            }
            Debug.DrawLine(left.position, hit.point,Color.red);
        }
        if (lerp == true)
        {
            elapsedFromMoved += Time.deltaTime;
        }
        bool[] hits = { fronthit, lefthit, righthit };
        //print(dir);
        int truths = 0;
        foreach(var i in hits) {//just to calculate how many target there are to its front and sides
            if (i) truths += 1;
        }
        if(truths > 1) {
            dir = Vector3.right;
        }
        if (dir.x==-1 &lerp== true && elapsedFromMoved>=duration)
        {//this is the lerping to the side so that the obstacle could move to the side but honestly this work only against one object and not that viable in the game,'
         //because right normally if the the object infront of it is somewhat has the same z value the thing will go haywire
            
            if (timeElapsed < lerpDuration02 )//lerping i gues
            {
                valueToLerp = Mathf.Lerp(transform.position.z, inital.z, timeElapsed / lerpDuration02);
                timeElapsed += Time.deltaTime;
            }
            else//stop lerping
            {
                valueToLerp = inital.z;
                timeElapsed = 0;
                elapsedFromMoved = 0;
                lerp = false;
            }
            
            transform.position = new Vector3(transform.position.x, transform.position.y, valueToLerp);//move to the destined location once done
            
        }
        if((transform.position.z>inital.z+2 )||( transform.position.z > inital.z + 2))//ensure that the player move at the right direction
        {
            dir = -Vector3.right;
            transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y, inital.z);
        }
    }
}
