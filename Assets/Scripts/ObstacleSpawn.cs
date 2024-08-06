using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawn : MonoBehaviour
{
    public GameObject[] obstacles;
    GameObject myobstacle;
    public int spawnamt;
    int enemiesspawned;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.CompareTag("Headphone")) {
            StartCoroutine(SpawnHeadphone());
        } 
        else {
            SpawnObstacle();
        }
        enemiesspawned = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if (myobstacle == null) SpawnObstacle();
    }
    void SpawnObstacle()
    {
        float rng = Random.Range(0f, 1f);
        if(obstacles.Length > 1) {
            if (rng > 0 && rng <= .12f) {
                myobstacle = Instantiate(obstacles[0], transform.position + new Vector3(Randomness(), 0, 0), transform.rotation);
            } 
            else if (rng > .3f && rng <= .5f) {
                myobstacle = Instantiate(obstacles[1], transform.position + new Vector3(Randomness(), 0, 0), transform.rotation);
            }
        } 
        else {
            if(rng > .3f && rng <= .5f)myobstacle = Instantiate(obstacles[0], transform.position + new Vector3(Randomness(), 0, 0), transform.rotation);
        }
        if(myobstacle != null) {
            myobstacle.GetComponent<Obstacle>().myspawner = this;
            myobstacle.transform.position += myobstacle.GetComponent<Obstacle>().spawnoffset;
            if (myobstacle.GetComponent<Collectible>() == null) myobstacle.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }
    IEnumerator SpawnHeadphone() {
        while (enemiesspawned <= spawnamt - 1) {
            myobstacle = Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform.position + new Vector3(Randomness(), 0, 0), transform.rotation);
            myobstacle.GetComponent<Obstacle>().myspawner = this;
            myobstacle.transform.position += myobstacle.GetComponent<Obstacle>().spawnoffset;
            if (myobstacle.GetComponent<Collectible>() == null) myobstacle.transform.rotation = Quaternion.Euler(0, -90, 0);
            enemiesspawned += 1;
            yield return new WaitForSeconds(2f);
        }
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
