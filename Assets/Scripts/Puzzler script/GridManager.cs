using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public List<GameObject> pathSquare;
    string currentGridSquare= "null";
    public Camera cam;
    float rayLength = 100;
    public float gridDistance = 1;
    RaycastHit hit;
    public LayerMask mask;
    public GameObject Arrow;
    // Start is called before the first frame update
    void Start()
    {
        pathSquare.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        foreach (GameObject G in pathSquare)
        {            
            Gizmos.DrawWireSphere(G.transform.position + new Vector3(0, 0.5f, 0), 0.5f);
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
                if (currentGridSquare == hit.collider.name) return;
                
                CheckAdjacent();
                //markingSquare();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //the player move from the start to the end
        }
    }

    void markingSquare()
    {
        //this function is used to color the marked squares
        for(int i =0;i < pathSquare.Count; i++)
        {
            GameObject arrow = Instantiate(Arrow, pathSquare.ToArray()[i].transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
        }
 
    }

    void CheckAdjacent()
    {
        currentGridSquare = hit.collider.name;
        if (pathSquare.Count == 0 && hit.collider.gameObject.layer ==7)
        {
            pathSquare.Add(hit.collider.gameObject);
        }
        else if (pathSquare.Count>=1 && CheckWhetherItIsAdjacent(hit.collider.gameObject))
        {
            if (!pathSquare.Contains(hit.collider.gameObject))pathSquare.Add(hit.collider.gameObject);            
            if (pathSquare.Count >= 2)
            {
                if (pathSquare.ToArray()[pathSquare.Count - 2] == hit.collider.gameObject && currentGridSquare==hit.collider.name)//if the raycast goes back to the same square
                {
                    pathSquare.RemoveAt(pathSquare.Count-1);
                }
            } 
        }
    }

    bool CheckWhetherItIsAdjacent(GameObject currentGrid)//this return true if the adjacent squares are marked
    {
        float numberOfNotMarkedSquareThatIsAdjacent= 0;
        float totalnumberOfAdjacentSquare = 0;
        RaycastHit hit;
        if (Physics.Raycast(currentGrid.transform.position+ new Vector3(gridDistance, 0.2f,0),
            transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, mask))//checking east grid
        {
            totalnumberOfAdjacentSquare += 1;
            if (pathSquare.ToArray()[pathSquare.Count - 1] == hit.collider.gameObject)//if the east grid is not marked, then add to the number of unmarked square  
            { numberOfNotMarkedSquareThatIsAdjacent += 1;    }
        }
        if (Physics.Raycast(currentGrid.transform.position + new Vector3(-gridDistance, 0.2f, 0),
            transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, mask))//checking west grid
        {
            totalnumberOfAdjacentSquare += 1;
            if (pathSquare.ToArray()[pathSquare.Count - 1] == hit.collider.gameObject)//if the west grid is not marked, then add to the number of unmarked square  
            {
                numberOfNotMarkedSquareThatIsAdjacent += 1;
            }
        }
        if (Physics.Raycast(currentGrid.transform.position + new Vector3(0, 0.2f, -gridDistance),
           transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, mask))//checking south grid
        {
            totalnumberOfAdjacentSquare += 1;
            if (pathSquare.ToArray()[pathSquare.Count - 1] == hit.collider.gameObject)//if the south grid is not marked, then add to the number of unmarked square  
            {
                numberOfNotMarkedSquareThatIsAdjacent += 1;

            }
        }
        if (Physics.Raycast(currentGrid.transform.position + new Vector3(0, 0.2f, gridDistance),
           transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, mask))//checking north grid
        {
            totalnumberOfAdjacentSquare += 1;
            if (pathSquare.ToArray()[pathSquare.Count-1]==hit.collider.gameObject)//if the norht grid is not marked, then add to the number of unmarked square  
            { numberOfNotMarkedSquareThatIsAdjacent += 1; }
        }
        print(numberOfNotMarkedSquareThatIsAdjacent);
        print(totalnumberOfAdjacentSquare);
        if (numberOfNotMarkedSquareThatIsAdjacent ==0 ) return false;//there is unmarked square than the total number of square, there is one marked squre adjacent
        else return true;
        
    }

    
}
