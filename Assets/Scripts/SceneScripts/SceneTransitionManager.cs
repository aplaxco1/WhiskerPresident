using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static void TransitionScene(int index, bool save = true, bool reload = true)
    {
        if (save && SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveToFile();
        }

        SceneManager.LoadScene(index);

        if (reload && SaveManager.Instance != null)
        {
            SaveManager.Instance.LoadFromFile();
        }
    }
    
    public static void TransitionNextScene(bool save = true, bool reload = false)
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        TransitionScene(index, save, reload);
    }
}
