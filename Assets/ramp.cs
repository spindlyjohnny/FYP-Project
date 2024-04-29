using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ramp : MonoBehaviour
{
    RaycastHit hit;
    public bool Passable;
    LayerMask mask;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(transform.TransformPoint(transform.position+ new Vector3(6.5f,-0.5f,0)), 0.1f);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckEdge()
    {
        Collider[] collide = Physics.OverlapSphere(transform.TransformPoint(transform.position + new Vector3(6.5f, -0.5f, 0)), 0.4f,mask);
        if (collide == null)
        {
            Passable = false;
        }
        else
        {
            Passable = true;
        }

    }
}
