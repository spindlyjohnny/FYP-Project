using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    public List<GameObject> pathSquaresForMovement;    
    [Range(0,10)]
    public float speed=1000;

    float minimumDistance = 0.01f;
    bool moving = false;
    GridManager grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GridManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (pathSquaresForMovement.Count <1)
            {
                moving = false;
                grid.moving = false;
                return;
            }
            else
            {
                Vector3 offset = transform.position - new Vector3(pathSquaresForMovement[0].transform.position.x,
                    transform.position.y, pathSquaresForMovement[0].transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(pathSquaresForMovement[0].transform.position.x,
                    transform.position.y, pathSquaresForMovement[0].transform.position.z), Time.deltaTime * speed);
                if(Vector3.SqrMagnitude(offset) < minimumDistance * minimumDistance)
                {
                    print("inside");
                    pathSquaresForMovement.RemoveAt(0);
                }

            }
        }
    }

    public void MovingNow(List<GameObject> pathing)
    {
        pathSquaresForMovement = pathing;
        moving = true;
    }
}
