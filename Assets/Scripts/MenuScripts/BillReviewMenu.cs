using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BillReviewMenu : MonoBehaviour
{
    public SceneTransition SceneTransition;

    public void BacktoOffice()
    {
        //SceneTransitionManager.TransitionPreviousScene();
        StartCoroutine(SceneTransition.ExitTransition(SceneManager.GetActiveScene().buildIndex - 1));
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
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
