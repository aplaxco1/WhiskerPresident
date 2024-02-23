using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    static public float timeValue = 120;
    public TMP_Text timerText;
    //private float gameTimer;
    //private bool gamePaused = false;

   
    void Update()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            ResetTimer();
        }

        /* if(!gamePaused)
        {
            gameTimer += Time.deltaTime;
        }
        else if (Time.timeScale == 0)
        {
            gamePaused = true;
        } */

        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        else if (timeToDisplay > 0)
        {
            timeToDisplay += 1;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public static void ResetTimer()
    {
        timeValue = 120;
    } 
}
