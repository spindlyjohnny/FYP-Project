using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public float fadetime;
    public Image blackscreen;
    // Start is called before the first frame update
    void Start()
    {
        blackscreen = GetComponentInChildren<Image>(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Appear() {
        blackscreen.gameObject.SetActive(true);
        //blackscreen.CrossFadeAlpha(1, fadetime, false);
        //if (blackscreen.color.a == 1) blackscreen.gameObject.SetActive(false);
    }
    public void Disappear() {
        blackscreen.gameObject.SetActive(false);
        //blackscreen.CrossFadeAlpha(0, fadetime, false);
        //if (blackscreen.color.a == 0) blackscreen.gameObject.SetActive(false);
    }
}
