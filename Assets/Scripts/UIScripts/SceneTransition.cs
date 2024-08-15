using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] public Animator transitionAnimator;
    void Awake()
    {
        
    }

    public IEnumerator ExitTransition(int nextScene) {
        transitionAnimator.SetTrigger("Exit");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(nextScene);
    }

    public IEnumerator ExitTransitionNoLoad() {
        yield return new WaitForSeconds(3);
        transitionAnimator.SetTrigger("Exit");
        yield return null;
    }
}
