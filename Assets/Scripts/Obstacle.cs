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
        if(Physics.Raycast(front.position,front.forward,out hit, .5f)) {
            if((transform.position - hit.transform.position).sqrMagnitude <= .5f) {
                dir = (transform.position - hit.transform.position).normalized; 
            }
            if ((transform.position - hit.transform.position).sqrMagnitude > .5f) {
                dir = Vector3.forward;
            }
            Debug.DrawLine(front.position, hit.point,Color.red);
        }
        if(Physics.Raycast(right.position,right.forward,out hit, .5f)) {
            if((transform.position - hit.transform.position).sqrMagnitude <= .5f) {
                dir = (transform.position - hit.transform.position).normalized;
            }
            if ((transform.position - hit.transform.position).sqrMagnitude > .5f) {
                dir = Vector3.forward;
            }
            Debug.DrawLine(right.position, hit.point,Color.red);
        }
        if(Physics.Raycast(left.position,left.forward,out hit, .5f)) {
            if((transform.position - hit.transform.position).sqrMagnitude <= .5f) {
                dir = (transform.position - hit.transform.position).normalized;
            }
            if ((transform.position - hit.transform.position).sqrMagnitude > .5f) {
                dir = Vector3.forward;
            }
            Debug.DrawLine(left.position, hit.point,Color.red);
        }
    }
}
