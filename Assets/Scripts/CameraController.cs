using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target,originalposition;
    public float smoothing = 1f,lookOffset;
    public Vector3 targetposition;
    public bool NPC,bus,train;
    // Start is called before the first frame update
    void Start() {
        NPC = false;
        bus = false;
        train = false;
        originalposition = FindObjectOfType<Player>().transform.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update() {
        if (NPC || bus) {
            targetposition = target.position;
        }
        /*else if (train) {

        }*/
        else {
            targetposition = new Vector3(target.position.x - lookOffset, transform.position.y, target.position.z);
        }
        transform.position = Vector3.Lerp(transform.position, targetposition, Time.deltaTime * smoothing);
    }
}