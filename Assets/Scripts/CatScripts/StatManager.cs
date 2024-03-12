using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{

    public static StatManager Instance;

    private int redStat;
    private int blueStat;
    private int greenStat;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (BillContentsManager.Instance != null)
        {
            BillContentsManager.Instance.WipeSavedBills();
        }
        BillController.StatVector startingStats = new BillController.StatVector();
        startingStats.RedStat = 50;
        startingStats.GreenStat = 50;
        startingStats.BlueStat = 50;
        AdjustStats(startingStats);
    }

    public void AdjustStats(BillController.StatVector statVector)
    {
        UpdateRedStat(statVector.RedStat);
        UpdateGreenStat(statVector.GreenStat);
        UpdateBlueStat(statVector.BlueStat);
    }

    private void UpdateRedStat(int factor)
    {
        redStat += factor;
        if (redStat <= 0)
        {
            TriggerLoss("Red Stat Loss");
        }
        redStat = Mathf.Clamp(redStat, 0, 100);
        StatTextManager.Instance.redStatText.text = "RedStat: " + redStat;
    }
    
    private void UpdateGreenStat(int factor)
    {
        greenStat += factor;
        if (greenStat <= 0)
        {
            TriggerLoss("Green Stat Loss");
        }
        greenStat = Mathf.Clamp(greenStat, 0, 100);
        StatTextManager.Instance.greenStatText.text = "GreenStat: " + greenStat;
    }
    
    private void UpdateBlueStat(int factor)
    {
        blueStat += factor;
        if (blueStat <= 0)
        {
            TriggerLoss("Blue Stat Loss");
        }
        blueStat = Mathf.Clamp(blueStat, 0, 100);
        StatTextManager.Instance.blueStatText.text = "BlueStat: " + blueStat;
    }

    public int GetRedStat()
    {
        return redStat;
    }
    
    public int GetBlueStat()
    {
        return blueStat;
    }
    
    public int GetGreenStat()
    {
        return greenStat;
    }

    public void TriggerLoss(string lossMessage)
    {
        print("GAME OVER: " + lossMessage);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
