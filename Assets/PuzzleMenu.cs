using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleMenu : MonoBehaviour
{
    Scene scene;
    // Start is called before the first frame update
    void Start()
    {
        
        scene = SceneManager.GetActiveScene();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(scene.name);
    }
}
