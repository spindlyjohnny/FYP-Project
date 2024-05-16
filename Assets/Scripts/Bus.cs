using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bus : Obstacle
{
    [SerializeField]bool moving = true;
    public bool transitioned;
    public Transform passengerpos;
    [SerializeField] Collider destination,transitiondestination;
    // Start is called before the first frame update
    void Start()
    {
        transitioned = false;
        //StartCoroutine(BusTransitioninator());
    }
    protected override void Update() {
        if(moving)transform.Translate(3 * Time.deltaTime * dir);
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other) {
        if (other == destination) {
            moving = false;
            StartCoroutine(BusTransitioninator());
        }
        else if (other == transitiondestination) {
            moving = false;
            transitioned = true;
        }
    }
    public IEnumerator BusTransitioninator() {
        //FindObjectOfType<FadeIn>().blackscreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        moving = true;
        //FindObjectOfType<FadeIn>().blackscreen.gameObject.SetActive(true);
        //FindObjectOfType<FadeIn>().Appear();
        FindObjectOfType<LevelManager>().loadingscreen.SetActive(true);
        yield return new WaitForSeconds(5f);
        FindObjectOfType<LevelManager>().loadingscreen.SetActive(false);
        //FindObjectOfType<FadeIn>().Disappear();
        //FindObjectOfType<FadeIn>().blackscreen.gameObject.SetActive(false);
    }
}
