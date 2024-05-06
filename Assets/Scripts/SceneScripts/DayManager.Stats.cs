using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DayManager : MonoBehaviour
{
    

    private int statA;
    private int statB;
    private int statC;

   
    public void AdjustStats(StatVector statVector)
    {
        UpdateStatA(statVector.StatA);
        UpdateStatB(statVector.StatB);
        UpdateStatC(statVector.StatC);
    }

    // Red Stat Correlates to the Bone Symbol RN
    private void UpdateStatA(int factor)
    {
        statA += factor;
        if (statA <= 0)
        {
            TriggerLoss("Red Stat Loss");
        }
        statA = Mathf.Clamp(statA, 0, 100);
        StatTextManager.Instance.statAText.text = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "dog-stat") + " " + statA;
    }
    
    // Green Stat Correlated to the Fish Symbol RN
    private void UpdateStatB(int factor)
    {
        statB += factor;
        if (statB <= 0)
        {
            TriggerLoss("StatB Loss");
        }
        statB = Mathf.Clamp(statB, 0, 100);
        StatTextManager.Instance.statBText.text = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "cat-stat") + " " + statC; // translate this
    }
    
    private void UpdateStatC(int factor)
    {
        statC += factor;
        if (statC <= 0)
        {
            TriggerLoss("StatC Loss");
        }
        statC = Mathf.Clamp(statC, 0, 100);
        StatTextManager.Instance.statCText.text = "StatC: " + statC;
    }

    public int GetStatA()
    {
        return statA;
    }
    
    public int GetStatB()
    {
        return statB;
    }
    
    public int GetStatC()
    {
        return statC;
    }

    public StatVector GetStats()
    {
        StatVector ret = new StatVector();
        ret.StatA = GetStatA();
        ret.StatB = GetStatB();
        ret.StatC = GetStatC();
        return ret;
    }
    
    public void SetStats(StatVector statVector)
    {
        statA = statVector.StatA;
        statB = statVector.StatB;
        statC = statVector.StatC;
    }

    public void TriggerLoss(string lossMessage)
    {
        print("GAME OVER: " + lossMessage);
    }

}
