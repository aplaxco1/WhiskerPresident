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
        StatVector startingStats = new StatVector();
        startingStats.RedStat = 50;
        startingStats.GreenStat = 50;
        startingStats.BlueStat = 50;
        AdjustStats(startingStats);
    }

    public void AdjustStats(StatVector statVector)
    {
        UpdateRedStat(statVector.RedStat);
        UpdateGreenStat(statVector.GreenStat);
        //UpdateBlueStat(statVector.BlueStat); // blue stat unused rn
    }

    // Red Stat Correlates to the Bone Symbol RN
    private void UpdateRedStat(int factor)
    {
        redStat += factor;
        if (redStat <= 0)
        {
            TriggerLoss("Red Stat Loss");
        }
        redStat = Mathf.Clamp(redStat, 0, 100);
        StatTextManager.Instance.redStatText.text = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "dog-stat") + " " + redStat;
    }
    
    // Green Stat Correlated to the Fish Symbol RN
    private void UpdateGreenStat(int factor)
    {
        greenStat += factor;
        if (greenStat <= 0)
        {
            TriggerLoss("Green Stat Loss");
        }
        greenStat = Mathf.Clamp(greenStat, 0, 100);
        StatTextManager.Instance.greenStatText.text = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "cat-stat") + " " + greenStat; // translate this
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

    public StatVector GetStats()
    {
        StatVector ret = new StatVector();
        ret.RedStat = GetRedStat();
        ret.GreenStat = GetGreenStat();
        ret.BlueStat = GetBlueStat();
        return ret;
    }
    
    public void SetStats(StatVector statVector)
    {
        redStat = statVector.RedStat;
        greenStat = statVector.GreenStat;
        blueStat = statVector.BlueStat;
    }

    public void TriggerLoss(string lossMessage)
    {
        print("GAME OVER: " + lossMessage);
    }

}
