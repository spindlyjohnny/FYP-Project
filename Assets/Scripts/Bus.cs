using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Bus : Obstacle
{
    [SerializeField]bool moving = true; // control when the bus moves
    [SerializeField] Collider destination; // bus stop
    // Start is called before the first frame update
    void Start()
    {
        
    }
    protected override void Update() {
        if(moving)transform.Translate(5 * Time.deltaTime * dir);
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other) {
        if (other == destination) {
            moving = false;
            StartCoroutine(BusTransitioninator());
        }
    }
    public IEnumerator BusTransitioninator() {
        yield return new WaitForSeconds(2f); // bus waits for a bit before moving off-screen
        moving = true;
        FindObjectOfType<LevelManager>().loadingScreen.gameObject.SetActive(true); // activate loading screen
        //FindObjectOfType<LevelManager>().loadingScreen.Play();
        //FindObjectOfType<LevelManager>().loadingscreen.GetComponent<Image>().sprite = FindObjectOfType<LevelManager>().loadingimgs[Random.Range(0, FindObjectOfType<LevelManager>().loadingimgs.Length)]; // set loading screen sprite
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(FindObjectOfType<LevelManager>().Move(2,LevelManager.Level.BusInterior));
    }
}
