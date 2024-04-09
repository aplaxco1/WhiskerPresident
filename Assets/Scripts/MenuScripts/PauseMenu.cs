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
        toggleTools(false);
        Time.timeScale = 0;
    }

    public void Pause_ControlsMenu()
    {
        toggleTools(false);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        toggleTools(true);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void toggleTools(bool toggle) {
        toolSwitchingManager.gameObject.GetComponent<WeaponSwitching>().enabled = toggle;
        foreach (Transform tool in toolSwitchingManager.transform) {
            tool.gameObject.GetComponent<ToolClass>().enabled = toggle;
        }
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
