using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DayManager
{
   
    public void AdjustStats(StatVector statVector)
    {
        UpdateStatA(statVector.StatA);
        UpdateStatB(statVector.StatB);
        UpdateStatC(statVector.StatC);
    }

    // Red Stat Correlates to the Bone Symbol RN
    private void UpdateStatA(int factor)
    {
        dayInfo.statA += factor;
        dayInfo.statA = Mathf.Clamp(dayInfo.statA, -100, 100);
        StatTextManager.Instance.statAText.text = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "dog-stat") + " " + dayInfo.statA;
    }
    
    // Green Stat Correlated to the Fish Symbol RN
    private void UpdateStatB(int factor)
    {
        dayInfo.statB += factor;
        dayInfo.statB = Mathf.Clamp(dayInfo.statB, -100, 100);
        StatTextManager.Instance.statBText.text = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "cat-stat") + " " + dayInfo.statB; // translate this
    }
    
    private void UpdateStatC(int factor)
    {
        dayInfo.statC += factor;
        dayInfo.statC = Mathf.Clamp(dayInfo.statC, -100, 100);
        StatTextManager.Instance.statCText.text = "StatC: " + dayInfo.statC;
    }

    public int GetStatA()
    {
        return dayInfo.statA;
    }
    
    public int GetStatB()
    {
        return dayInfo.statB;
    }
    
    public int GetStatC()
    {
        return dayInfo.statC;
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
        dayInfo.statA = statVector.StatA;
        dayInfo.statB = statVector.StatB;
        dayInfo.statC = statVector.StatC;
    }



}
