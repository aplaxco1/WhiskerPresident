using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class StatVector
{
        public int StatA;
        public int StatB;
        public int StatC;

        // Changed This For Localization Purposes - Autumn
        public string StringConversion()
        {
            string ret = "";
            ret += UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "dog-stat") + " " + StatA + "\n";
            ret += UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "cat-stat") + " " + StatB + "\n";
            // ret += "Red: " + StatA + "\n";
            // ret += "Green: " + StatB + "\n";
            // ret += "Blue: " + StatC + "\n";
            return ret;
        }

        public StatVector()
        {
            
        }
}
