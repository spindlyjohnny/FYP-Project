using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainObstacle : Interactable
{
    public GameObject moveNPC;
    public Transform NPClocation;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void MoveNPC() {
        moveNPC.transform.position = Vector3.Lerp(transform.position, NPClocation.position, 1 * Time.deltaTime);
    }
    // Update is called once per frame
    
}
