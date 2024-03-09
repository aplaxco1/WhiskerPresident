using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject toolSwitchingManager;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        toolSwitchingManager.SetActive(false);
        Time.timeScale = 0;
    }

    public void Pause_ControlsMenu()
    {
        toolSwitchingManager.SetActive(false);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        toolSwitchingManager.SetActive(true);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Settings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
