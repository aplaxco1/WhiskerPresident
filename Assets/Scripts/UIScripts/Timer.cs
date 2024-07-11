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
    public TMP_Text timerText;
    public GameObject timesUpPopup;
    public GameObject laserPointer; // to pause
    public VolumeManager volumeManager;
    public NewBillMovement billMovementScript;

    private float flashTimer = 0.75f;
    private float flashDuration = 0.25f;

    private bool timesUpOver = false;

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
        else
        {
            StartCoroutine(TimesUp());
        }

        if (timeValue <= 30) {
            Flash();
        }

        if (timesUpOver) {
            timesUpOver = false;
            timeValue = 0;
            if (billMovementScript) {
                foreach(GameObject bill in billMovementScript.bills) {
                    bill.GetComponentInChildren<BillController>().UninitializeBill();
                }
            }
            ResetTimer();
            DayManager.Instance.DayEnd();
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
        Time.timeScale = 1;
    }

    private void Flash()
    {
        flashTimer -= Time.deltaTime;
        if (flashTimer < 0) {
            SetTextDisplay(false);
            flashDuration -= Time.deltaTime;
            if (flashDuration < 0) {
                SetTextDisplay(true);
                flashTimer = 0.75f;
                flashDuration = 0.25f;
            }
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

        timesUpOver = true;
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
