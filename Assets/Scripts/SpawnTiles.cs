using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTiles : MonoBehaviour
{
    public Transform spawnpt;
    public GameObject tile;
    bool once=true;
    // Start is called before the first frame update
    void Start()
    {
        //Destroy(Instantiate(tile, spawnpt.position, spawnpt.rotation), 2f);
        //for (int i = 0; i < 1; i++) {

        //}
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //spawn tiles at designated area by either calling the manager or spawning the tiles itself
        if (other.GetComponent<Player>())//only the player collision will spawn the tile
        {
            if (once)
            {
                Instantiate(tile, spawnpt.position, spawnpt.rotation);
            }

        }
    }
}
