using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayer : MonoBehaviour
{
    Image img;
    [SerializeField] Sprite[] checkbox;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Set() {
        if (img.sprite == checkbox[0]) { // select sprite
            img.sprite = checkbox[1];
            if (gameObject.CompareTag("Boy")) {
                PlayerPrefs.SetInt("Player", 0); // boy
            } 
            else {
                PlayerPrefs.SetInt("Player", 1); // girl
            }
        } 
        else if (img.sprite == checkbox[1]) { // checked sprite
            img.sprite = checkbox[0];
        }
    }
}
