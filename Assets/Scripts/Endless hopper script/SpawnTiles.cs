using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTiles : MonoBehaviour
{
    public Transform spawnpt;
    public GameObject tile;
    bool once=true;
    [SerializeField]GameObject NPC;
    GameObject go;
    // Start is called before the first frame update
    void Start()
    {
        NPC = GetComponentInChildren<NPC>(true).gameObject;
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
                go = Instantiate(tile, spawnpt.position, spawnpt.rotation);
                int rng = Random.Range(0, 2);
                print(rng);
                if (rng == 0) {
                    go.GetComponent<SpawnTiles>().NPC.SetActive(false);
                }
                else {
                    go.GetComponent<SpawnTiles>().NPC.SetActive(true);
                    go.GetComponent<SpawnTiles>().NPC.transform.position = go.GetComponent<SpawnTiles>().NPC.GetComponent<NPC>().startpos.position;
                }
                once = false;
                //if(!FindObjectOfType<CameraController>().NPC)Destroy(go, 100);
            }

        }
    }
    private void OnTriggerExit(Collider other) {
        //NPC.transform.position = NPC.GetComponent<NPC>().startpos;
        Destroy(gameObject);
    }
}
