using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : Obstacle
{
    [SerializeField]Vector3 originalsize;
    public Vector3 trainsize; // change size of collectible when in train since it's too big otherwise
    public AudioClip collectsound;
    // Start is called before the first frame update
    void Start()
    {
        originalsize = transform.localScale;
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    protected override void Update() {
        if (myspawner == null) Destroy(gameObject);
        //else Destroy(gameObject, 20f);
        if (levelManager.level == LevelManager.Level.MRT || levelManager.level == LevelManager.Level.BusInterior) transform.localScale = trainsize; // change size in mrt level
        else transform.localScale = originalsize;
        transform.Rotate(0, 60 * Time.deltaTime, 0);
    }
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Player>()) {            
            AudioManager.instance.PlaySFX(collectsound);
            Destroy(gameObject); 
        }
    }
}
