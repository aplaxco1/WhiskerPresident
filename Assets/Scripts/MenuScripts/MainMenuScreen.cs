using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;

public class MainMenuScreen : MonoBehaviour
{

    IEnumerator Start()
    {
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;
        // silly little way to make sure unity uses the first locale as the default
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
    }

    public void PlayGame()
    {
        Timer.timeValue = 120;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("Tutorial 1");
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
