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
        
        //if (gameObject.tag == "Headphone")
        //{
        //    float random = Random.Range(-3.5f, 2.5f);
        //    transform.position = new Vector3(transform.position.x , transform.position.y, transform.position.z +random);
        //}
        SpawnObstacle();

    }

    // Update is called once per frame
    void Update()
    {
        //if (myobstacle == null) SpawnObstacle();
    }
    void SpawnObstacle()
    {
        myobstacle = Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform.position + new Vector3(Randomness(), 0, 0), transform.rotation);
        myobstacle.GetComponent<Obstacle>().myspawner = this;
        myobstacle.transform.position += myobstacle.GetComponent<Obstacle>().spawnoffset;
        if (myobstacle.GetComponent<Collectible>() == null) myobstacle.transform.rotation = Quaternion.Euler(0, -90, 0);
    }
    int Randomness()
    {
        int random = 0;
       
        if (obstacles.Length == 1)
        {
            return random;
        }
        else
        {
            int indexer = Random.Range(0, 2);
            if (indexer == 0)
            {
                random = Random.Range(-6, 0);
            }
            else
            {
                random = Random.Range(1, 6);
            }
            return random;
        }
        
    }
}
