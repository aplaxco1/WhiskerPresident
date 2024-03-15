using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangChanger : MonoBehaviour
{

    public void ToggleLang()
    {
        if (PlayerPrefs.GetString("Lang") != "EN")
        {
            PlayerPrefs.SetString("Lang", "EN");
        }
        else if(PlayerPrefs.GetString("Lang") != "ZH")
        {
            PlayerPrefs.SetString("Lang", "ZH");
        }

    }
}
