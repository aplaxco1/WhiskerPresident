using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class SinkVector
{
        public int SinkA;
        public int SinkB;
        public int SinkC;

        public string StringConversion()
        {
            string ret = "";
            ret += SinkA + SinkB + SinkC;
            // TODO: string conversion
            // ret += UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "dog-stat") + " " + StatA + "\n";
            // ret += UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "cat-stat") + " " + StatB + "\n";
            return ret;
        }

        public SinkVector()
        {
            
        }
}
