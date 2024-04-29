using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : SceneLoader {
    public bool gameover;
    public GameObject gameoverscreen;
    public Slider energyslider;
    // Start is called before the first frame update
    void Start()
    {
        gameover = false;
        gameoverscreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameover)gameoverscreen.SetActive(true);
    }
    public void RestartLevel() {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
