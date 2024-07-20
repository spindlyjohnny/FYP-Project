using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Stop()
    {
        anim.SetBool("Stop", true);
    }   
    public void Continue()
    {
        anim.SetBool("Stop", false);
    }
}
