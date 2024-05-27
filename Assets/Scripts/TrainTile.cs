using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTile : Tile
{
    public TrainObstacle obstacleNPCs;
    // Start is called before the first frame update
    protected override void Start() {
        obstacleNPCs = transform.Find("Obstacle Group").GetComponent<TrainObstacle>();
        base.Start();
    }
    // Update is called once per frame
    protected override void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            obstacleNPCs.MoveNPC();
        }
        base.Update();
    }
}
