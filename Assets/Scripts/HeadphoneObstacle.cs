    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadphoneObstacle : Obstacle
{
    public float distanceBeforeTurningBack=10f;
    Vector3 InitalPosition;    
    float distanceBetween;
    public bool walkingRight = true;
    Vector3 direction;
    private void Start()
    {
        InitalPosition = transform.position;
    }
    protected override void Update()
    {
        RaycastHit hit;
        float distanceBetween = Vector3.SqrMagnitude(InitalPosition - transform.position);
        //print(distanceBetween);
        if((distanceBetween >= Mathf.Pow(distanceBeforeTurningBack,2) || Physics.Raycast(front.position, front.forward, out hit, .5f)) && walkingRight)
        {
            walkingRight = false;
        }
        else if(distanceBetween <=1*0.1 && walkingRight==false)
        {
            walkingRight = true;
        }

        if (walkingRight)
        {
            direction = ((transform.position + Vector3.forward) - transform.position).normalized;
            var targettedRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targettedRotation, 540*Time.deltaTime );
            transform.Translate(0 * Time.deltaTime * Vector3.forward,Space.World);
        }
        else
        {
            direction = (InitalPosition - transform.position).normalized;
            var targettedRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targettedRotation, 540 *Time.deltaTime);
            transform.Translate(0 * Time.deltaTime * -Vector3.forward,Space.World);
            
        }
        if (!myspawner.gameObject.activeInHierarchy) Destroy(gameObject);
        //else Destroy(gameObject, 15);
    }
}
