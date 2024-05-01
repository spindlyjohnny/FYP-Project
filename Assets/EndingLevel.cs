using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingLevel : MonoBehaviour
{
    public GameObject menu;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if (other.tag=="Player")
        {
            print("ending");
            Time.timeScale = 0;
            menu.SetActive(true);
        }
    }
}
