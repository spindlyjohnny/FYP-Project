using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public List<GameObject> pathSquare;
    GameObject[] oBject;
    string currentGridSquare= "null";
    public Camera cam;
    float rayLength = 100;
    RaycastHit hit;
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        pathSquare.Clear();
    }

    // Update is called once per frame
    void Update()
    {      
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, rayLength, mask))
            {
                print(hit.collider.tag);
                CheckAdjacent();
            }
        }
    }

    void CheckAdjacent()
    {

        if (pathSquare.Count == 0 && hit.collider.gameObject.layer ==7)
        {
            currentGridSquare = hit.collider.name;
            pathSquare.Add(hit.collider.gameObject);
        }
        else if (pathSquare.Count>0 && CheckWhetherItIsAdjacent(hit.collider.gameObject))
        {
            if(pathSquare.Find(i => i = hit.collider.gameObject) && currentGridSquare==hit.collider.name)//if the raycast goes back to the same 
            {
                pathSquare.RemoveAt(pathSquare.Count-1);
            }
        }
    }

    bool CheckWhetherItIsAdjacent(GameObject currentGrid)
    {
        float numberOfSquareThatIsNotAdjacent= 0;
        RaycastHit hit;
        if (Physics.Raycast(currentGrid.transform.position+ new Vector3(1,0.2f,0),
            transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, mask))//checking east grid
        {
            if(pathSquare.Find(i => i = hit.collider.gameObject) ==false)//if the grid  
            { numberOfSquareThatIsNotAdjacent += 1; }
        }
        if (Physics.Raycast(currentGrid.transform.position + new Vector3(-1, 0.2f, 0),
            transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, mask))//checking west grid
        {
            if (pathSquare.Find(i => i = hit.collider.gameObject) == false)
            { numberOfSquareThatIsNotAdjacent += 1; }
        }
        if (Physics.Raycast(currentGrid.transform.position + new Vector3(0, 0.2f, -1),
           transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, mask))//checking south grid
        {
            if (pathSquare.Find(i => i = hit.collider.gameObject) == false)
            { numberOfSquareThatIsNotAdjacent += 1; }
        }
        if (Physics.Raycast(currentGrid.transform.position + new Vector3(0, 0.2f, 1),
           transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, mask))//checking north grid
        {
            if (pathSquare.Find(i => i = hit.collider.gameObject) == false)
            { numberOfSquareThatIsNotAdjacent += 1; }
        }
        if (numberOfSquareThatIsNotAdjacent == 4) return false;
        else return true;
        
    }

    
}
