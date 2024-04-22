using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform target;
    public float smoothing = 1f;
    public float heightDifference=5f;
    public float distanceAwayX = 2;
    public float distanceAway = 5;
    public Vector3 targetposition, originalposition;
    public bool NPC;
    // Start is called before the first frame update
    void Start()
    {
        NPC = false;
        originalposition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        targetposition = NPC ? target.position : new Vector3(target.position.x + distanceAwayX, target.position.y +heightDifference, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetposition, Time.deltaTime * smoothing);
    }
}
