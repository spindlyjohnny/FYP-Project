using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class sliderFill : MonoBehaviour
{
    public GameObject fill;
    Slider slide;
    private void Start()
    {
        slide = GetComponent<Slider>();
        fill.SetActive(true);
    }
    public void Zero()
    {
        if (slide.value < 0.05)
        {
            fill.SetActive(false);
        }
    }
}
