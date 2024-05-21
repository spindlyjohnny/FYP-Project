using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTile : Tile
{
    public GameObject obstacleNPCs;
    // Start is called before the first frame update

    // Update is called once per frame
    protected override void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {

        }
        base.Update();
    }
}
