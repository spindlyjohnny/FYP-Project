using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampGrid : MonoBehaviour
{
    RaycastHit hit;
    public bool Passable;
    public GameObject UpperBox;
    public GameObject LowerObject;
    LayerMask maskGrid;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(transform.TransformPoint(new Vector3(5f,0,0)), 0.1f);
    }
    // Start is called before the first frame update
    void Start()
    {
        if(Physics.Raycast(transform.position,Vector3.down,Mathf.Infinity,))
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckEdge()
    {
        Collider[] collide = Physics.OverlapSphere(transform.TransformPoint(new Vector3(6.5f, -0.5f, 0)), 0.4f,this.mask);
        if (collide != null)
        {
            
        }

    }
}
