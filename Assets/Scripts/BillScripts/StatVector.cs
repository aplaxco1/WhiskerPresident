using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatVector
{
        public int RedStat;
        public int GreenStat;
        public int BlueStat;

        // Changed This For Localization Purposes - Autumn
        public string StringConversion()
        {
            string ret = "";
            ret += UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "dog-stat") + " " + RedStat + "\n";
            ret += UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "cat-stat") + " " + GreenStat + "\n";
            // ret += "Red: " + RedStat + "\n";
            // ret += "Green: " + GreenStat + "\n";
            // ret += "Blue: " + BlueStat + "\n";
            return ret;
        }
}
