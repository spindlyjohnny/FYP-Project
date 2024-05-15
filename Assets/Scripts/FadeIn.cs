using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public float fadetime;
    Image blackscreen;
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
        blackscreen.CrossFadeAlpha(1, fadetime, false);
    }
    public void Disappear() {
        blackscreen.CrossFadeAlpha(0, fadetime, false);
    }
}
