using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    public GameObject panel;
    [HideInInspector]
    public bool stop=false;
    public static bool isBusInterrior=false, isMrt = false;
    public TextMeshProUGUI text;
    [TextArea(3,10)]
    public string[] tutorialTexts;
    static int two=0;
    Player player;
    LevelManager manager;
    PauseScreen pause;
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
        pause = FindObjectOfType<PauseScreen>();
        player = FindObjectOfType<Player>();
        manager = FindObjectOfType<LevelManager>();
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            PlayerPrefs.SetInt("Tutorial", 0);
            two = 0; 
        }
        if (PlayerPrefs.GetInt("Tutorial") == 1) return;
        if(state==TutorialState.defaultInstruction && manager.level == LevelManager.Level.Bus)
        {
            two = 0;
            state = (TutorialState)1;
            StartCoroutine(FirstTutorial());  
        }
        if( manager.level == LevelManager.Level.BusInterior && state == (TutorialState)7 && isBusInterrior == false)
        {
            StartCoroutine(SeventhTutorial());
        }
        if (manager.level == LevelManager.Level.MRT && state == (TutorialState)7 && isMrt==false)
        {
            StartCoroutine(EightTutorial());
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
            if(Input.GetButtonDown("Fire1")|| Input.GetButtonDown("Fire2"))
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
        print("next");
        yield return new WaitForSeconds(2);
        print("next");
        stop = true;
        state = (TutorialState)2;
        Time.timeScale = 0;
        panel.SetActive(true);
        for(int i = 0; i < 5; i++)
        {
            state = (TutorialState)(i+2);
            text.text = tutorialTexts[i+1];//the end index will be 3
            yield return new WaitForSecondsRealtime(0.5f);
            
            while (true)
            {
                
                print("tutoriaLING");
                if (Input.GetButton("Fire1") || Input.GetButton("Fire2"))
                {
                    
                    break;
                }
                yield return null;
            }
            
        }
        print("end");
        state = (TutorialState)7;
        panel.SetActive(false);
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
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
            {
                break;
            }
            yield return null;
        }
        panel.SetActive(false);
        state = (TutorialState)7;
        Time.timeScale = 1;
        stop = false;
        two += 1;
        if (two >= 2)
        {
            PlayerPrefs.SetInt("Tutorial", 1);
        }
        isBusInterrior = true;
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
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
            {
                break;
            }
            yield return null;
        }
        panel.SetActive(false);
        state = (TutorialState)7;
        Time.timeScale = 1;
        stop = false;
        two += 1;
        if (two >= 2)
        {
            PlayerPrefs.SetInt("Tutorial", 1);
        }
        isMrt = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}