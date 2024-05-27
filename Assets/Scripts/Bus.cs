using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        if(moving)transform.Translate(5 * Time.deltaTime * dir);
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
            FindObjectOfType<LevelManager>().loadingscreen.SetActive(false);
            transform.SetParent(null);
        }
    }
    public IEnumerator BusTransitioninator() {
        //FindObjectOfType<FadeIn>().blackscreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        moving = true;
        //FindObjectOfType<FadeIn>().blackscreen.gameObject.SetActive(true);
        //FindObjectOfType<FadeIn>().Appear();
        FindObjectOfType<LevelManager>().loadingscreen.SetActive(true);
        FindObjectOfType<LevelManager>().loadingscreen.GetComponent<Image>().sprite = FindObjectOfType<LevelManager>().loadingimgs[UnityEngine.Random.Range(0, FindObjectOfType<LevelManager>().loadingimgs.Length)];
        //yield return new WaitForSeconds(5f);

        //FindObjectOfType<FadeIn>().Disappear();
        //FindObjectOfType<FadeIn>().blackscreen.gameObject.SetActive(false);
    }
}
