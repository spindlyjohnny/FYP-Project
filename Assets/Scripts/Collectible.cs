using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : Obstacle
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update() {
        if (myspawner == null) Destroy(gameObject);
        else Destroy(gameObject, 20f);
        transform.Rotate(0, 60 * Time.deltaTime, 0);
    }
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Player>()) Destroy(gameObject);
    }
}
