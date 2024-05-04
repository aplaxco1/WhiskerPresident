using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class MainMenuScreen : MonoBehaviour
{

    public GameObject langScreen;

    async void Start() {
        if (langScreen && LangChanger.langScreenSeen) {
            langScreen.SetActive(false);
        }
        // INITIALIZE ANALYTICS DATA COLLECTION
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
    }

    public void PlayGame()
    {
        LangChanger.langScreenSeen = true;
        Timer.timeValue = 120;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
