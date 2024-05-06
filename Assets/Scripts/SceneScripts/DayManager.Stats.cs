using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DayManager
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
        statA = Mathf.Clamp(statA, -100, 100);
        StatTextManager.Instance.statAText.text = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "dog-stat") + " " + statA;
    }
    
    // Green Stat Correlated to the Fish Symbol RN
    private void UpdateStatB(int factor)
    {
        statB += factor;
        statB = Mathf.Clamp(statB, -100, 100);
        StatTextManager.Instance.statBText.text = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "cat-stat") + " " + statC; // translate this
    }
    
    private void UpdateStatC(int factor)
    {
        statC += factor;
        statC = Mathf.Clamp(statC, -100, 100);
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



}
