using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;

public class MainMenuScreen : MonoBehaviour
{

    public GameObject langScreen;

    void Start() {
        if (langScreen && LangChanger.langScreenSeen) {
            langScreen.SetActive(false);
        }
    }

    public void PlayGame()
    {
        LangChanger.langScreenSeen = true;
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
