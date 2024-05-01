using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle : MonoBehaviour
{
    RaycastHit hit;
    public LayerMask mask;
    CapsuleCollider collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CapsuleCollider>();
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 10000, mask))
        {
            print(hit.collider.gameObject.name);
            hit.collider.gameObject.layer = 9;
            transform.position = hit.collider.gameObject.transform.position + new Vector3(0, collider.radius*20, 0);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
