using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    GameObject selectedObject=null;
    RaycastHit hit;
    public Camera cam;
    public LayerMask obstacleMask;
    public LayerMask Mask;
    GridManager grid;
    CapsuleCollider collider;
    Ray ray;
    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GridManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10000, obstacleMask))
            {
                selectedObject = hit.collider.gameObject;
                collider = hit.collider.GetComponent<CapsuleCollider>();
            }
        }
       
        if (Input.GetMouseButton(1))
        {
            if (selectedObject == null) return;
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10000, Mask))
            {
                selectedObject.transform.position = hit.collider.gameObject.transform.position + new Vector3(0, collider.radius, 0);
            }
        }
        
        if (Input.GetMouseButtonUp(1))//upon release the obstacle is deselected
        {
            if (selectedObject == null) return;
            print("yes");
            if (Physics.Raycast(selectedObject.transform.position, -Vector3.up, out hit, 100,Mask))
            {
                print(hit.collider.name);
                hit.collider.gameObject.layer = 9;
            }
            grid.RemoveDownList();
            selectedObject = null;
        }
    }
}
