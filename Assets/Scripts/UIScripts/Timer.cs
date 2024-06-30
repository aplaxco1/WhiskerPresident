using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    static public float timeValue = 120;
    static public float timesUpTime = 5;
    public TMP_Text timerText;
    public GameObject timesUpPopup;
    public GameObject laserPointer; // to pause
    public VolumeManager volumeManager;
    public NewBillMovement billMovementScript;

    private float flashTimer;
    private float flashDuration = 1f;
    //public GameObject flashing_Label;
    //public float interval;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ResetTimer();

        //AnalyticsEvents.playerScreenSettings(); // CHECKS IF PLAYER PLAYING GAME FULLSCREEN OR WINDOWED
        //InvokeRepeating("FlashLabel", 0, interval);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            timeValue = 2;
        }
        
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else if (timeValue == 60)
        {
            Flash();
        }
        else if (timeValue == 30)
        {
            Flash();
        }
        else
        {
            StartCoroutine(TimesUp());
        }

        //AnalyticsEvents.checkFramerate(); // CHECKS FRAMERATE DURING GAMEPLAY FOR ANALYTICS

        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
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
        timesUpTime = 5;
        Time.timeScale = 1;
    }

    private void Flash()
    {
        if (timeValue != 0)
        {
            timeValue = 0;
            DisplayTime(timeValue);
        }

        if (flashTimer >= 0 && flashTimer <= 30)
        {
            flashTimer = flashDuration;
        }
        else if (flashTimer >= flashDuration / 2)
        {
            flashTimer -= Time.deltaTime;
            SetTextDisplay(false);
        }
        else
        {
            flashTimer -= Time.deltaTime;
            SetTextDisplay(true);
        }
    }

    private void SetTextDisplay(bool enabled)
    {
        timerText.enabled = enabled;
    }

    IEnumerator TimesUp() {
        Time.timeScale = 0;
        timesUpPopup.SetActive(true);
        laserPointer.SetActive(false);
        volumeManager.ChangeSoundsVolume(-100);

        StatVector sinkValues = new StatVector();
        sinkValues.StatA = SaveManager.Instance.currentSaveData.dayInfo.sinkA;
        sinkValues.StatB = SaveManager.Instance.currentSaveData.dayInfo.sinkB;
        sinkValues.StatC = SaveManager.Instance.currentSaveData.dayInfo.sinkC;
        StatTextManager.initializePopups(sinkValues);

        yield return new WaitForSecondsRealtime(5);

        timeValue = 0;
        if (billMovementScript) {
            foreach(GameObject bill in billMovementScript.bills) {
                bill.GetComponentInChildren<BillController>().UninitializeBill();
            }
        }
        ResetTimer();
        Debug.Log(SaveManager.Instance.currentSaveData.dayInfo.day);
        DayManager.Instance.DayEnd();
        //if (SaveManager.Instance.currentSaveData.dayInfo.day == 1) { AnalyticsEvents.tutorialCompleted(); } // TEMP WAY TO SEND ANALYTICS EVENT IF FIRST ROUND COMPLETE
    }

    /*void FlashLabel()
    {
        if(flashing_Label.activeSelf)
        {
            flashing_Label.SetActive(false);
        }
        else
        {
            flashing_Label.SetActive(true);
        }
    }*/
}
