using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsResolution : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ResolutionDefault();
    }

    public void ResolutionDefault()
    {
        Resolution1();
    }

    public void Resolution1()
    {
        Settings.Resolution res = Settings.Resolution1;
        Screen.SetResolution(res.width, res.height, FullScreenMode.FullScreenWindow);
    }

    public void Resolution2()
    {
        Settings.Resolution res = Settings.Resolution2;
        Screen.SetResolution(res.width, res.height, FullScreenMode.FullScreenWindow);
    }

    public void Resolution3()
    {
        Settings.Resolution res = Settings.Resolution3;
        Screen.SetResolution(res.width, res.height, FullScreenMode.FullScreenWindow);    }

    public void Resolution4()
    {
        Settings.Resolution res = Settings.Resolution4;
        Screen.SetResolution(res.width, res.height, FullScreenMode.FullScreenWindow);    }

    public void Resolution5()
    {
        Settings.Resolution res = Settings.Resolution5;
        Screen.SetResolution(res.width, res.height, FullScreenMode.FullScreenWindow);    }
}
