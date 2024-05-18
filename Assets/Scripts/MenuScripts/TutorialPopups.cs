using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class TutorialPopups : MonoBehaviour
{
    // this is the best way i could get this to work
    public List<GameObject> tutorialMenusEN;
    public List<GameObject> tutorialMenusZH;

    public GameObject laserPointer;

    // NOTE: Once we have actual popups made, we can start programming this for real. Right now, all this does is set
    // our controls menu to active at the start of the scene and pauses the game. It also has to handle
    // localization stuff because I cant think of any other way to do it LOL.

    // Start is called before the first frame update
    void Start()
    {   
        if (SaveManager.Instance.currentSaveData.dayInfo.day <= 1) {
            if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.GetLocale("en-US"))
            {
                tutorialMenusEN[0].SetActive(true);
            }
            else if(LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.GetLocale("zh"))
            {
                tutorialMenusZH[0].SetActive(true);
            }
            // pause game
            laserPointer.SetActive(false);
            Time.timeScale = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {   
    }
}
