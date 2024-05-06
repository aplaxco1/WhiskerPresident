using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static void TransitionScene(int index, bool save = true, bool reload = false)
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
}
