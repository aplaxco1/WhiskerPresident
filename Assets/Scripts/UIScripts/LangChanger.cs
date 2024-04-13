using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LangChanger : MonoBehaviour
{

    public List<GameObject> enAssets;
    public List<GameObject> zhAssets;

    IEnumerator Start()
    {
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;
        // silly little way to make sure unity uses the first locale as the default
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];

        ToggleLangAssets();
    }

    public void ToggleLangAssets()
    {
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.GetLocale("en-US"))
        {
            Debug.Log("IN ENGLISH");
            foreach (GameObject obj in enAssets) {
                obj.SetActive(true);
            }
            foreach (GameObject obj in zhAssets) {
                obj.SetActive(false);
            }
        }
        else if(LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.GetLocale("zh"))
        {
            Debug.Log("IN CHINESE");
            foreach (GameObject obj in enAssets) {
                obj.SetActive(false);
            }
            foreach (GameObject obj in zhAssets) {
                obj.SetActive(true);
            }
        }

    }
}
