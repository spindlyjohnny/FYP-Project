using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target,originalposition/*,trainposition*/;
    public float smoothing = 1f,lookOffset,defaultoffset,interioroffset;
    public Vector3 targetposition;
    public bool NPC,bus;
    // Start is called before the first frame update
    void Start() {
        NPC = false; // if player is talking to NPC, move to the NPC camera postion
        bus = false;// same thing but for bus
        originalposition = FindObjectOfType<LevelManager>().level == LevelManager.Level.Bus ? FindObjectOfType<Player>().transform.Find("Original Cam Pos")
            : FindObjectOfType<Player>().transform.Find("Bus Interior Cam Pos"); // camera angle for bus level and interior

        target = FindObjectOfType<Player>().transform;
        //trainposition = FindObjectOfType<Player>().transform.Find("Train Cam Pos").transform; // camera angle for train level
        lookOffset = defaultoffset; // defaultoffset is camera offset in bus level. camera is closer in train level so there is a need for a trainoffset variable.
    }

    // Update is called once per frame
    void Update() {
        if (NPC || bus) {
            targetposition = target.position; // when NPC or bus is true, camera exactly tracks the target. The bus and NPC prefabs have empty gameObjects as children that act as targets for the camera to track.
        }
        else {
            targetposition = new Vector3(target.position.x - lookOffset,transform.position.y, target.position.z);
        }
        transform.position = Vector3.Lerp(transform.position, targetposition, Time.deltaTime * smoothing);
    }
}