using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    List<Transform> gridSquare;
    GameObject[] oBject;
    public Camera cam;
    float rayLength = 100;
    RaycastHit hit;
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        gridSquare.Clear();
        GameObject[] grid = GameObject.FindGameObjectsWithTag("Grid");
        for(int i=0; i < grid.Length; i++)
        {
            gridSquare.Add(grid[i].transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, rayLength, mask))
            {

            }
        }
    }

    void CheckAdjacent()
    {

    }
}
