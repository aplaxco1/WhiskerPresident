using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsResolution : MonoBehaviour
{

    public static SettingsResolution Instance;
    public SettingsData.Resolution currentResolution;

    public RenderReplacementShaderToTexture outlines;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //ResolutionDefault();
    }

    public void ResolutionDefault()
    {
        Resolution1();
    }

    public void Resolution1()
    {
        SetRes(SettingsData.Resolution1);
    }

    public void Resolution2()
    {
        SetRes(SettingsData.Resolution2);
    }

    public void Resolution3()
    {
        SetRes(SettingsData.Resolution3);    
    }

    public void Resolution4()
    {
        SetRes(SettingsData.Resolution4);
    }

    public void Resolution5()
    {
        SetRes(SettingsData.Resolution5);
    }

    public void SetRes(SettingsData.Resolution res)
    {
        if (outlines) { outlines.renderOutlines(); }
        currentResolution = res;
        Screen.SetResolution(res.width, res.height, FullScreenMode.FullScreenWindow);    
    }
}
