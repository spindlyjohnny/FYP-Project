using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;
    public float smoothing = 1f;
    public Vector3 targetposition,originalposition;
    public bool NPC,train;
    // Start is called before the first frame update
    void Start() {
        NPC = false;
        train = false;
        originalposition = transform.position;
    }

    // Update is called once per frame
    void Update() {
        targetposition = NPC ? target.position: new Vector3(target.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetposition, Time.deltaTime * smoothing);
    }
}