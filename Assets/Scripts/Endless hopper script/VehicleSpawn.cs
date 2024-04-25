using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawn : MonoBehaviour
{
    public GameObject[] vehicles;
    GameObject myvehicle;
    // Start is called before the first frame update
    void Start()
    {
        SpawnVehicle();
    }

    // Update is called once per frame
    void Update()
    {
        if (myvehicle == null) SpawnVehicle();
    }
    void SpawnVehicle() {
        myvehicle = Instantiate(vehicles[Random.Range(0, vehicles.Length)], transform.position, transform.rotation);
        myvehicle.GetComponent<Vehicle>().myspawner = this;
    }
}
