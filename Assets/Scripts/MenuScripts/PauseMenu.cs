using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject volumeSliders;
    [SerializeField] GameObject laserPointer;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        if(laserPointer != null)
        {
            laserPointer.SetActive(false);
        }
        AudioListener.pause = true;
        Time.timeScale = 0;
    }

    public void Pause_ControlsMenu()
    {
        if (laserPointer != null)
        {
            laserPointer.SetActive(false);
        }
        AudioListener.pause = true;
        Time.timeScale = 0;
    }

    public void Continue()
    {
        if (laserPointer != null)
        {
            laserPointer.SetActive(true);
        }
        pauseMenu.SetActive(false);
        volumeSliders.SetActive(false);
        settingsMenu.SetActive(false);
        AudioListener.pause = false;
        Time.timeScale = 1;
    }

    // public void toggleTools(bool toggle) {
    //     toolSwitchingManager.gameObject.GetComponent<WeaponSwitching>().enabled = toggle;
    //     foreach (Transform tool in toolSwitchingManager.transform) {
    //         tool.gameObject.GetComponent<ToolClass>().enabled = toggle;
    //     }
    // }

    public void Settings()
    {
        pauseMenu.SetActive(false);
        volumeSliders.SetActive(true);
        settingsMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Quit()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene("MainMenu");
    }
}
