using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangDetector : MonoBehaviour
{
    public Canvas EnCanvas;
    public Canvas ZhCanvas;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetString("Lang") != "EN")
        {
            if (PlayerPrefs.GetString("Lang") != "ZH")
            {
                PlayerPrefs.SetString("Lang", "EN");
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void LangDetect()
    {
        if (PlayerPrefs.GetString("Lang") == "EN")
        {
            //return "EN";
            EnCanvas.gameObject.SetActive(true);
            ZhCanvas.gameObject.SetActive(false);
        }
        else if(PlayerPrefs.GetString("Lang") == "ZH")
        {
            // return "ZH";
            EnCanvas.gameObject.SetActive(false);
            ZhCanvas.gameObject.SetActive(true);
        }
    }
}
