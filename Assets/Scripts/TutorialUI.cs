using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    public GameObject panel;
    [HideInInspector]
    public bool stop=false;
    public TextMeshProUGUI text;
    [TextArea(3,10)]
    public string[] tutorialTexts;
    Player player;
    LevelManager manager;
    public enum TutorialState
    {
        defaultInstruction=0,
        firstInstruction=1,
        secondInstruction=2,
        thirdInstruction=3,
        fourthInstruction=4,
        fifthInstruction=5,
        sixthInstruction=6,
        seventhInstruction=7,
        finalInstruction=8,
        finish=9
    }

    public static TutorialState state= (TutorialState)0;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        manager = FindObjectOfType<LevelManager>();
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            PlayerPrefs.SetInt("Tutorial", 0);
        }
        if (PlayerPrefs.GetInt("Tutorial") == 1) return;
        if(state==TutorialState.defaultInstruction && manager.level == LevelManager.Level.Bus)
        {
            state = (TutorialState)1;
            StartCoroutine(FirstTutorial());  
        }
        if( manager.level == LevelManager.Level.MRT && state == (TutorialState)7)
        {
            StartCoroutine(SeventhTutorial());
        }
        if (manager.level == LevelManager.Level.BusInterior && state == (TutorialState)8)
        {
            StartCoroutine(SeventhTutorial());
        }
    }
    IEnumerator FirstTutorial()
    {

        yield return new WaitForSeconds(1);
        stop = true;
        state = (TutorialState)1;//1 us the first intsruction
        Time.timeScale = 0;
        panel.SetActive(true);
        text.text = tutorialTexts[0];
        while (true)
        {
            if(Input.GetButton("Fire1")|| Input.GetButton("Fire2"))
            {
                player.Movement();//this is to ensure that the player moves which is to show that the movement works
                break;
            }
            yield return null;
        }
        panel.SetActive(false);
        state = (TutorialState)2;
        Time.timeScale = 1;
        stop = false;
        StartCoroutine(LoopingTutorial());
    }

    IEnumerator LoopingTutorial()
    {
        yield return new WaitForSeconds(2);
        stop = true;
        state = (TutorialState)2;
        Time.timeScale = 0;
        panel.SetActive(true);
        for(int i = 0; i < 3; i++)
        {
            state = (TutorialState)(i+2);
            text.text = tutorialTexts[i+1];//the end index will be 3
            while (true)
            {
                if (Input.GetButton("Fire1") || Input.GetButton("Fire2"))
                {                
                    break;
                }
                yield return null;
            }
            
        }
        state = (TutorialState)5;
        panel.SetActive(false);
        Time.timeScale = 1;
        stop = false;
    }

    public IEnumerator FifthAndSixthTutorial()
    {
        stop = true;
        Time.timeScale = 0;
        panel.SetActive(true);
        for (int i = 4; i < 6; i++)
        {
            state = (TutorialState)(i+1);
            text.text = tutorialTexts[i];
            while (true)
            {
                if (Input.GetButton("Fire1") || Input.GetButton("Fire2"))
                {
                    break;
                }
                yield return null;
            }

        }
        panel.SetActive(false);
        state = (TutorialState)7;
        Time.timeScale = 1;
        stop = false;
    }
    IEnumerator SeventhTutorial()
    {
        yield return new WaitForSeconds(1);
        stop = true;
        state = (TutorialState)1;//1 us the first intsruction
        Time.timeScale = 0;
        panel.SetActive(true);
        text.text = tutorialTexts[6];
        while (true)
        {
            if (Input.GetButton("Fire1") || Input.GetButton("Fire2"))
            {
                break;
            }
            yield return null;
        }
        panel.SetActive(false);
        state = (TutorialState)8;
        Time.timeScale = 1;
        stop = false;
    }

    IEnumerator EightTutorial()
    {
        yield return new WaitForSeconds(1);
        stop = true;
        Time.timeScale = 0;
        panel.SetActive(true);
        text.text = tutorialTexts[7];
        while (true)
        {
            if (Input.GetButton("Fire1") || Input.GetButton("Fire2"))
            {
                break;
            }
            yield return null;
        }
        panel.SetActive(false);
        state = (TutorialState)9;
        Time.timeScale = 1;
        stop = false;
        PlayerPrefs.SetInt("Tutorial", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}