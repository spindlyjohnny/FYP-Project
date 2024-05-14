using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ObstacleSpawn myspawner;
    public Vector3 spawnoffset;
    bool moving = true;
    public Vector3 dir;
    [SerializeField]Collider destination;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(moving)transform.Translate(3 * Time.deltaTime * dir);
        if (myspawner == null && destination == null) Destroy(gameObject);
        else Destroy(gameObject, 15f);
    }
    private void OnTriggerEnter(Collider other) {
        if (destination && other == destination)moving = false;
    }
}
