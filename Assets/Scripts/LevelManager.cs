using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : SceneLoader {
    public bool gameover;
    public GameObject gameoverscreen;
    public Slider energyslider;
    SpawnTiles tiles;
    public int score;
    public TMP_Text scoretext;
    // Start is called before the first frame update
    void Start()
    {
        tiles = FindObjectOfType<SpawnTiles>();
        tiles.Spawn(8);
        gameover = false;
        gameoverscreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameover)gameoverscreen.SetActive(true);
        scoretext.text = "Score:" + score.ToString();
    }
    public void RestartLevel() {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
