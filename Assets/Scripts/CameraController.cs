using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;
    public float smoothing = 1f;
    public Vector3 targetposition;
    //public bool followtarget;
    // Start is called before the first frame update
    void Start() {
        //followtarget = true;
    }

    // Update is called once per frame
    void Update() {
        targetposition = new Vector3(target.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetposition, Time.deltaTime * smoothing);
        //if (followtarget) {
            
        //}
    }
}