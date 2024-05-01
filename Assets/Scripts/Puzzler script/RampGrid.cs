using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampGrid : MonoBehaviour
{
    RaycastHit hit;
    public bool Passable;
    public GameObject UpperBox;
    public GameObject LowerObject;
    public LayerMask maskGrid;
    public LayerMask rampGrid;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(transform.TransformPoint(new Vector3(5f, 0, 0)), 0.3f);
        Gizmos.DrawWireCube(transform.TransformPoint(new Vector3(5f, 0, 0)), new Vector3(0.5f, 0.5f, 0.5f));
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Physics.Raycast(transform.position, Vector3.down,out hit, Mathf.Infinity, rampGrid))
        {
            this.transform.SetParent(hit.collider.gameObject.transform, true);
            CheckEdge();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckEdge()
    {
        if(Physics.BoxCast(transform.TransformPoint(new Vector3(5f, 0, 0)),new Vector3(0.6f,0.5f,0.6f),new Vector3(1,0,0),out hit,Quaternion.identity,Mathf.Infinity,maskGrid))
        {
            UpperBox = hit.collider.gameObject;
        }
        else
        {
            UpperBox = null;
        }
    }
}
