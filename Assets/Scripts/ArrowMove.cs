using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    Vector3 originalpos;
    // Start is called before the first frame update
    void Start()
    {
        originalpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(originalpos.y,originalpos.y + .3f,Mathf.PingPong(Time.time,1)), transform.position.z);
    }
}
