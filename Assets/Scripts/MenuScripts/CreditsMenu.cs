using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenu : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void NextMorning()
    {
        SceneTransitionManager.TransitionPreviousScene();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
