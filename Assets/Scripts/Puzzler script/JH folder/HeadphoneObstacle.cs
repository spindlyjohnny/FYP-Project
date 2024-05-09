using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadphoneObstacle : MonoBehaviour
{
    public ObstacleSpawn myspawner;
    public Vector3 spawnoffset;
    public float distanceBeforeTurningBack=10f;
    Vector3 InitalPosition;    
    float distanceBetween;
    public bool walkingRight = true;
    Vector3 direction;
    private void Start()
    {
        InitalPosition = transform.position;
    }

    protected virtual void Update()
    {
        float distanceBetween = Vector3.SqrMagnitude(InitalPosition - transform.position);
        //print(distanceBetween);
        if(distanceBetween >= distanceBeforeTurningBack * distanceBeforeTurningBack && walkingRight==true)
        {
            walkingRight = false;
        }
        else if(distanceBetween <=1*1 && walkingRight==false)
        {
            walkingRight = true;
        }

        if (walkingRight)
        {
            direction = ((transform.position + Vector3.forward) - transform.position).normalized;
            var targettedRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targettedRotation, 360*Time.deltaTime );
            transform.Translate(3 * Time.deltaTime * Vector3.forward,Space.World);
        }
        else
        {
            direction = (InitalPosition - transform.position).normalized;
            var targettedRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targettedRotation, 360 *Time.deltaTime);
            transform.Translate(3 * Time.deltaTime * -Vector3.forward,Space.World);
            
        }
        /*
        if (myspawner == null) Destroy(gameObject);
        else Destroy(gameObject, 15);*/
    }
}
