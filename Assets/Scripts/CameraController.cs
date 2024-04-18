using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;
    public float smoothing = 1f;
    //public bool followtarget;
    // Start is called before the first frame update
    void Start() {
        //followtarget = true;
    }

    // Update is called once per frame
    void Update() {
        Vector3 targetposition = new Vector3(transform.position.x, transform.position.y, target.position.z);
        transform.position = Vector3.Lerp(transform.position, targetposition, Time.deltaTime * smoothing);
        //if (followtarget) {

        //}
    }
}