using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridManager : MonoBehaviour
{
    public List<GameObject> pathSquare;
    string currentGridSquare= "null";
    public Camera cam;
    float rayLength = 100;
    public float gridDistance = 1;
    public int allowedGridUsed = 10;
    public bool moving=false;
    RaycastHit hit;
    public LayerMask mask;
    public LayerMask rampMask;
    public GameObject Arrow;
    public Material currentGridMaterial;
    public Material pastGridMaterial;
    public Material starterGridMaterial;
    public TextMeshProUGUI text;
    PlayerMoving movement;
    // Start is called before the first frame update
    void Start()
    {
        pathSquare.Clear();
        movement = FindObjectOfType<PlayerMoving>();
        text.text = "Grids left: " +(allowedGridUsed-pathSquare.Count).ToString();
    }

    // Update is called once per frame
    void Update()
    {      
        if (Input.GetMouseButton(0))
        {
            if (moving) return;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, rayLength, mask))
            {
                if (currentGridSquare == hit.collider.name) return;
                
                CheckAdjacent();
                MarkingSquare();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (pathSquare.Count < 2) return; 
            if (moving) return;
            moving = true;
            //the player move from the start to the end
            movement.MovingNow(pathSquare);
        }
    }

    void MarkingSquare()
    {
        text.text = "Grids left: " + (allowedGridUsed - pathSquare.Count).ToString();
        //this function is used to color the marked squares
        foreach (GameObject i in pathSquare)
        {
            i.GetComponent<MeshRenderer>().enabled = true;
        }
        for(int i =0; i < pathSquare.Count; i++)
        {
            if(i != 0  && i !=pathSquare.Count-1)
            {
                pathSquare[i].GetComponent<Renderer>().material = pastGridMaterial;
            }else if(i == pathSquare.Count - 1)
            { 
                pathSquare[i].GetComponent<Renderer>().material = starterGridMaterial;
            }
            else
            {
                pathSquare[i].GetComponent<Renderer>().material = currentGridMaterial;
            }
        }
        
    }

    void CheckAdjacent()
    {
        currentGridSquare = hit.collider.name;//so that this event trigger once per block
        if (pathSquare.Count == 0 && hit.collider.gameObject.layer ==7 && (allowedGridUsed - pathSquare.Count) > 0)//add the starting grid
        {
            pathSquare.Add(hit.collider.gameObject);
        }
        else if (pathSquare.Count>=1 && CheckWhetherItIsAdjacent(hit.collider.gameObject))
        {
            if (!pathSquare.Contains(hit.collider.gameObject) && (allowedGridUsed - pathSquare.Count)>0 )pathSquare.Add(hit.collider.gameObject);//if the list does not contain a copy of the same object, add the gameobject to the list
            if (pathSquare.Count >= 2 && pathSquare.ToArray()[pathSquare.Count - 2] == hit.collider.gameObject && currentGridSquare == hit.collider.name)//to ensure no index range exception
            {//if the raycast goes back to the same square
                
            pathSquare[pathSquare.Count - 1].GetComponent<MeshRenderer>().enabled = false;
            pathSquare.RemoveAt(pathSquare.Count-1);
                
            } 
        }
    }

    //work in progress
    bool IsItFromARamp(GameObject currentGrid)
    {
        bool isThereARamp=false;
        RaycastHit hit;
        if (Physics.Raycast(currentGrid.transform.position + new Vector3(gridDistance, 100f, 0), transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, rampMask))//checking east grid
        {
            if (pathSquare.ToArray()[pathSquare.Count - 1] == hit.collider.gameObject)//if the east grid is not marked, then add to the number of unmarked square  
            {
                isThereARamp = true;
            }
        }
        if (Physics.Raycast(currentGrid.transform.position + new Vector3(-gridDistance, 100f, 0),transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, rampMask))//checking west grid
        {
            if (pathSquare.ToArray()[pathSquare.Count - 1] == hit.collider.gameObject)//if the west grid is not marked, then add to the number of unmarked square  
            {
                isThereARamp = true;
            }
        }
        if (Physics.Raycast(currentGrid.transform.position + new Vector3(0, 100f, -gridDistance),transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, rampMask))//checking south grid
        {
            if (pathSquare.ToArray()[pathSquare.Count - 1] == hit.collider.gameObject)//if the south grid is not marked, then add to the number of unmarked square  
            {
                isThereARamp = true;
            }
        }
        if (Physics.Raycast(currentGrid.transform.position + new Vector3(0, 100f, gridDistance),transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, rampMask))//checking north grid
        {
            if (pathSquare.ToArray()[pathSquare.Count - 1] == hit.collider.gameObject)//if the norht grid is not marked, then add to the number of unmarked square  
            {
                isThereARamp = true;
            }
        }
        return true;
    }

    bool CheckWhetherItIsAdjacent(GameObject currentGrid)//this return true if the adjacent squares are marked
    {
        float numberOfNotMarkedSquareThatIsAdjacent= 0;
        float totalnumberOfAdjacentSquare = 0;
        RaycastHit hit;
        if (Physics.Raycast(currentGrid.transform.position+ new Vector3(gridDistance, 100f, 0),
            transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, mask))//checking east grid
        {
            totalnumberOfAdjacentSquare += 1;
            if (pathSquare.ToArray()[pathSquare.Count - 1] == hit.collider.gameObject)//if the east grid is not marked, then add to the number of unmarked square  
            { numberOfNotMarkedSquareThatIsAdjacent += 1;    }
        }
        if (Physics.Raycast(currentGrid.transform.position + new Vector3(-gridDistance, 100f, 0),
            transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, mask))//checking west grid
        {
            totalnumberOfAdjacentSquare += 1;
            if (pathSquare.ToArray()[pathSquare.Count - 1] == hit.collider.gameObject)//if the west grid is not marked, then add to the number of unmarked square  
            {
                numberOfNotMarkedSquareThatIsAdjacent += 1;
            }
        }
        if (Physics.Raycast(currentGrid.transform.position + new Vector3(0, 100f, -gridDistance),
           transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, mask))//checking south grid
        {
            totalnumberOfAdjacentSquare += 1;
            if (pathSquare.ToArray()[pathSquare.Count - 1] == hit.collider.gameObject)//if the south grid is not marked, then add to the number of unmarked square  
            {
                numberOfNotMarkedSquareThatIsAdjacent += 1;

            }
        }
        if (Physics.Raycast(currentGrid.transform.position + new Vector3(0, 100f, gridDistance),
           transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, mask))//checking north grid
        {
            totalnumberOfAdjacentSquare += 1;
            if (pathSquare.ToArray()[pathSquare.Count-1]==hit.collider.gameObject)//if the norht grid is not marked, then add to the number of unmarked square  
            { numberOfNotMarkedSquareThatIsAdjacent += 1; }
        }
        if (numberOfNotMarkedSquareThatIsAdjacent ==0 ) return false;//there is unmarked square than the total number of square, there is one marked squre adjacent
        else return true;
        
    }

    public void RemoveDownList()
    {
        int selectedNumber = -100;
        for(int i=0; i < pathSquare.Count; i++)
        {
            print(pathSquare[i].gameObject.layer);
            if (pathSquare[i].gameObject.layer == 9)
            {
                selectedNumber = i;
            }
            
        }
        if (selectedNumber > -10)
        {
            for(int i= pathSquare.Count - 1; i >= selectedNumber; i--)
            {
                pathSquare[i].GetComponent<Renderer>().enabled = false;
                pathSquare.RemoveAt(i);
            }
        }
        MarkingSquare();
    }
    
}
