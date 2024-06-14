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
    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Translate(3 * Time.deltaTime * dir);
        if (myspawner == null) Destroy(gameObject);
        Sensors();
        //else Destroy(gameObject, 15f);
    }
   void Sensors() {
        RaycastHit hit;
        Vector3 temp;
        bool fronthit = false, lefthit = false, righthit = false;
        bool[] hits = { fronthit, lefthit, righthit };
        // casts rays in 3 directions. if any ray detects an object, move away from object until there is nothing blocking.
        // current problem is that ai doesnt know what to do if more than 1 ray detects an object. unsure of how to do this efficiently.
        if(Physics.Raycast(front.position,front.forward,out hit, .5f)) {
            fronthit = true;
            temp = (transform.position - hit.transform.position);
            if (temp.sqrMagnitude <= .5f) {
                dir = new Vector3(temp.x,0,temp.z).normalized;
            }
            if (temp.sqrMagnitude > .5f) {
                dir = Vector3.forward;
                fronthit = false;
            }
            Debug.DrawLine(front.position, hit.point,Color.red);
        }
        if(Physics.Raycast(right.position,right.forward,out hit, .5f)) {
            righthit = true;
            temp = (transform.position - hit.transform.position);
            if (temp.sqrMagnitude <= .5f) {
                dir = new Vector3(temp.x, 0, temp.z).normalized;
            }
            if (temp.sqrMagnitude > .5f) {
                dir = Vector3.forward;
                righthit = false;
            }
            Debug.DrawLine(right.position, hit.point,Color.red);
        }
        if(Physics.Raycast(left.position,left.forward,out hit, .5f)) {
            lefthit = true;
            temp = (transform.position - hit.transform.position);
            if (temp.sqrMagnitude <= .5f) {
                dir = new Vector3(temp.x, 0, temp.z).normalized;
            }
            if (temp.sqrMagnitude > .5f) {
                dir = Vector3.forward;
                lefthit = false;
            }
            Debug.DrawLine(left.position, hit.point,Color.red);
        }
        int truths = 0;
        foreach(var i in hits) {
            if (i) truths += 1;
        }
        if(truths > 1) {
            dir = Vector3.back;
        }
    }
}
