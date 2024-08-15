using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfGame : MonoBehaviour
{

    static public bool looseGame = false;
    static public bool winGame = false;

    public GameObject loseText;
    public GameObject winText;

    public SceneTransition SceneTransition;

    // Start is called before the first frame update
    void Start()
    {
        if (looseGame) {
            loseText.SetActive(true);
            winText.SetActive(false);
        }
        if (winGame) {
            loseText.SetActive(false);
            winText.SetActive(true);
        }
    }

    public void BackToMainMenu() {
        looseGame = false;
        winGame = false;
        StartCoroutine(SceneTransition.ExitTransition(0));
        //SceneManager.LoadScene(0);
    }

    
}
