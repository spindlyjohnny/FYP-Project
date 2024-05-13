using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTile : Tile
{
    // Start is called before the first frame update
    protected override void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
