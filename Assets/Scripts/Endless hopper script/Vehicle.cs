using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public VehicleSpawn myspawner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(9 * Time.deltaTime * Vector3.left);
        if (myspawner == null) Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other) {
        if (other.name == "Vehicle Endpoint") Destroy(gameObject);
    }
}