using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Newspaper : MonoBehaviour
{

    public TMP_Text newsText1;
    public TMP_Text newsText2;

    // Start is called before the first frame update
    void Start()
    {  
        float avg = calulateEconomyAverage();
        if (avg >= 35f) {
            newsText1.text = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "news-high-1");
            newsText2.text = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "news-high-2");
        }
        else if (avg <= 10f) {
            newsText1.text = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "news-low-1");
            newsText2.text = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "news-low-2");
        }
        else {
            newsText1.text = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "news-mid-1");
            newsText2.text = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "news-mid-2");
        }
    }

    // Update is called once per frame
    void Update()
    {   
    }

    float calulateEconomyAverage() {
        return (DayManager.Instance.dayInfo.statA + DayManager.Instance.dayInfo.statB + DayManager.Instance.dayInfo.statC) / 3f;
    }
}
