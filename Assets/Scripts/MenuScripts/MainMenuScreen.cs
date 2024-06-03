using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuScreen : MonoBehaviour
{

    public GameObject langScreen;
    public GameObject continueScreen;
    public SaveManager SaveManager;
    public AudioSource bgm;

    void Start() {
        if (langScreen && LangChanger.langScreenSeen) {
            langScreen.SetActive(false);
        }
    }

    void Update() {
        if (!langScreen.activeSelf) {
            if (!bgm.isPlaying) { bgm.Play(); }
        }
    }

    public void PlayGame()
    {
        LangChanger.langScreenSeen = true;
        Timer.timeValue = 120;
        // if no save data just transition to next screen without popup
        if (SaveManager.Instance.currentSaveData.dayInfo.day <= 1) { ContinueGame(); }
        else { continueScreen.SetActive(true); }
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void ContinueGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneTransitionManager.TransitionNextScene();
    }

//     public void StartNewGame() {
//         //File.Delete(Application.persistentDataPath + "/save1.sav");
//         SaveManager.ResetSaveData();
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
//         //SceneTransitionManager.TransitionNextScene();
//     } 
}
