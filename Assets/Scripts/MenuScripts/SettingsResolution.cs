using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsResolution : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
    }

    public void Resolution1()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
    }

    public void Resolution2()
    {
        Screen.SetResolution(1600, 900, FullScreenMode.FullScreenWindow);
    }

    public void Resolution3()
    {
        Screen.SetResolution(1440, 900, FullScreenMode.FullScreenWindow);
    }

    public void Resolution4()
    {
        Screen.SetResolution(1366, 768, FullScreenMode.FullScreenWindow);
    }

    public void Resolution5()
    {
        Screen.SetResolution(1280, 1024, FullScreenMode.FullScreenWindow);
    }
}
