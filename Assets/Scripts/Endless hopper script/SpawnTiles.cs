using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTiles : MonoBehaviour
{
    public Transform spawnpt;
    public GameObject tile;
    //bool once=true;
    [SerializeField]GameObject NPC;
    [SerializeField]GameObject[]bus_stop,buildings;
    GameObject go;
    int shiftfactor;
    // Start is called before the first frame update
    void Start()
    {
        NPC = GetComponentInChildren<NPC>(true).gameObject;
        shiftfactor = 0;
        //Destroy(Instantiate(tile, spawnpt.position, spawnpt.rotation), 2f);
        //for (int i = 0; i < 10; i++) {
        //    Instantiate(tile, spawnpt.position, spawnpt.rotation);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTiles[] currenttiles = FindObjectsOfType<SpawnTiles>();
        shiftfactor = currenttiles.Length == 1 ? 0 : 21; // shiftfactor spawns tiles 21 units ahead because when player enters trigger, there are 3 tiles in front. each tile is 7 units long on the x-axis
    }

    private void OnTriggerEnter(Collider other)
    {
        //SpawnTiles[] currenttiles = FindObjectsOfType<SpawnTiles>();
        //print(currenttiles.Length);
        //if(currenttiles.Length % 4 == 0) {
        //    shiftfactor = 21;
        //}
        //spawn tiles at designated area by either calling the manager or spawning the tiles itself
        if (other.GetComponent<Player>())//only the player collision will spawn the tile
        {
            for (int x = 0; x < 4; x++) { // spawn 4 tiles at a time
                // spawn tile at spawn point + size of tile * order that tile was spawned
                go = Instantiate(tile, spawnpt.position + new Vector3(7 * x, 0, 0) + new Vector3(shiftfactor,0,0), spawnpt.rotation);
                SpawnTiles mytile = go.GetComponent<SpawnTiles>();
                float rng = Random.Range(0f, 1f);
                //print(rng);
                if (rng == 0) {
                    mytile.NPC.SetActive(false);
                    foreach (GameObject i in mytile.bus_stop) i.SetActive(false);
                    foreach (GameObject i in mytile.buildings) i.SetActive(false);
                } else if (rng <= 0.25f && rng > 0) {
                    mytile.NPC.SetActive(true);
                    foreach (GameObject i in mytile.bus_stop) i.SetActive(true);
                    mytile.NPC.transform.localPosition = mytile.NPC.GetComponent<NPC>().startpos;
                } else if (0.25f < rng && rng <= 0.75f) {
                    foreach (GameObject i in mytile.buildings) i.SetActive(true);
                    mytile.NPC.SetActive(false);
                    foreach (GameObject i in mytile.bus_stop) i.SetActive(false);
                }
            }
            //if (once)
            //{

            //    once = false;
            //    //if(!FindObjectOfType<CameraController>().NPC)Destroy(go, 100);
            //}
        }
    }
    private void OnTriggerExit(Collider other) {
        //NPC.transform.position = NPC.GetComponent<NPC>().startpos;
        Destroy(gameObject);
    }
}
