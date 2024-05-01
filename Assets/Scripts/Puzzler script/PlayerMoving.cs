using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    public List<GameObject> pathSquaresForMovement;    
    [Range(0,10)]
    public float speed=1000;
    public GameObject gameoverMenu;
    Transform targettedPosition=null;
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
        Moving();
    }

    void Moving()
    {
        if (moving==false) return;        
        if (pathSquaresForMovement.Count < 1)
        {
            moving = false;
            grid.moving = false;
            StartCoroutine(Losing());
            return;
        }
        else
        {
            
            Vector3 offset = new Vector3(pathSquaresForMovement[0].transform.position.x,
                transform.position.y, pathSquaresForMovement[0].transform.position.z) - transform.position;
            Vector3 direction = Vector3.RotateTowards(transform.forward, offset, 45 * Time.deltaTime,0.0f);
            transform.rotation = Quaternion.LookRotation(direction);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(pathSquaresForMovement[0].transform.position.x,
                transform.position.y, pathSquaresForMovement[0].transform.position.z), Time.deltaTime * speed);

            if (Vector3.SqrMagnitude(offset) < minimumDistance * minimumDistance)
            {
                pathSquaresForMovement.RemoveAt(0);
            }

        }
        
    }

    IEnumerator Losing()
    {
        yield return new WaitForSeconds(2);
        Time.timeScale = 0;
        gameoverMenu.SetActive(true);
    }

    public void MovingNow(List<GameObject> pathing)
    {
        pathSquaresForMovement = pathing;
        moving = true;
    }
}
