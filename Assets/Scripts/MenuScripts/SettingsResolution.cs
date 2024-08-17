using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsResolution : MonoBehaviour
{

    public static SettingsResolution Instance;
    public SettingsData.Resolution currentResolution;

    public RenderReplacementShaderToTexture outlines;
    public Tiling environmentHalftoneTiler;
    public Tiling presHalftoneTiler;

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
        currentResolution = res;
        StartCoroutine(updateRes());    
    }

    private IEnumerator updateRes() {
        Screen.SetResolution(currentResolution.width, currentResolution.height, FullScreenMode.FullScreenWindow);
        yield return null;

        if (environmentHalftoneTiler){ environmentHalftoneTiler.TileChildren(); }
        if (presHalftoneTiler) { presHalftoneTiler.TileChildren();}
        if (outlines) { outlines.renderOutlines(); }
    }
}
