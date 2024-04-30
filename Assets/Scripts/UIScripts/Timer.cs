using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    static public float timeValue = 120;
    public TMP_Text timerText;
    public NewBillMovement billMovementScript;

    private float flashTimer;
    private float flashDuration = 1f;
    //public GameObject flashing_Label;
    //public float interval;

    void Start()
    {
        ResetTimer();
        //InvokeRepeating("FlashLabel", 0, interval);
    }

    void Update()
    {
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
            Flash();
            timeValue = 0;
            if (billMovementScript) {
                foreach(GameObject bill in billMovementScript.bills) {
                    bill.GetComponentInChildren<BillController>().UninitializeBill();
                }
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            ResetTimer();
        }

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
