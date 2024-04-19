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
        SettingsData.Resolution res = SettingsData.Resolution1;
        Screen.SetResolution(res.width, res.height, FullScreenMode.FullScreenWindow);
    }

    public void Resolution2()
    {
        SettingsData.Resolution res = SettingsData.Resolution2;
        Screen.SetResolution(res.width, res.height, FullScreenMode.FullScreenWindow);
    }

    public void Resolution3()
    {
        SettingsData.Resolution res = SettingsData.Resolution3;
        Screen.SetResolution(res.width, res.height, FullScreenMode.FullScreenWindow);    }

    public void Resolution4()
    {
        SettingsData.Resolution res = SettingsData.Resolution4;
        Screen.SetResolution(res.width, res.height, FullScreenMode.FullScreenWindow);    }

    public void Resolution5()
    {
        SettingsData.Resolution res = SettingsData.Resolution5;
        Screen.SetResolution(res.width, res.height, FullScreenMode.FullScreenWindow);    }
}
