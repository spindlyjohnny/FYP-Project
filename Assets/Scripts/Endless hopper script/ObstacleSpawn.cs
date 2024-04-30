using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawn : MonoBehaviour
{
    public GameObject[] obstacles;
    GameObject myobstacle;
    // Start is called before the first frame update
    void Start()
    {
        SpawnObstacle();
    }

    // Update is called once per frame
    void Update()
    {
        if (myobstacle == null) SpawnObstacle();
    }
    void SpawnObstacle() {
        myobstacle = Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform.position, transform.rotation);
        if(myobstacle.GetComponent<Obstacle>() != null)myobstacle.GetComponent<Obstacle>().myspawner = this;
    }
}
