using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor.SearchService;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSequenceTwo : MonoBehaviour {
    public enum TutorialStep {switchTools, toggleLaser, finish}
    [SerializeField] private GameObject completeButton;
    [SerializeField] private GameObject promptSwitch;
    [SerializeField] private GameObject promptLaser;
    public static TutorialStep currentStep;

    // Start is called before the first frame update
    void Start()
    {
        currentStep = (TutorialStep)0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentStep)
        {
            case TutorialStep.switchTools:
                Debug.Log("Switch Tools!");
                if (!promptSwitch.activeSelf)
                {
                    promptSwitch.SetActive(true);
                    promptLaser.SetActive(false);
                }
                break;
            case TutorialStep.toggleLaser:
                Debug.Log("Toggle Laser!");
                if (!promptLaser.activeSelf)
                {
                    promptSwitch.SetActive(false);
                    promptLaser.SetActive(true);
                }
                break;
            //case TutorialStep.smackPaper:
              //  Debug.Log("Smack Paper!");
                //break;
            case TutorialStep.finish:
                Debug.Log("Finished!");
                if(!completeButton.activeSelf) {
                    promptLaser.SetActive(false);
                    completeButton.SetActive(true);
                }
                break;

        }
        //HighlightObject(stackOfBills, billHighlighted);
        //if (billMovementScript.billOut) {
        //    paper = billMovementScript.currBill.GetComponentInChildren<Renderer>().gameObject;
        //    HighlightObject(paper, paperHighlighted);
        //}
        //HighlightObject(bin, binHighlighted);
    }

    void HighlightObject(GameObject gameObject, bool isHighlighted)
    {
        if (isHighlighted)
        {
            gameObject.GetComponent<MeshRenderer>().material
                      .SetFloat("_Highlight", 1);
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material
                      .SetFloat("_Highlight", 0);
        }
    }

    public static bool NextStepInTutorial(int stepNumber) {
        if(stepNumber == (int)currentStep + 1)
        {
            currentStep = (TutorialStep)stepNumber;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void NextTutorial() {
        SceneManager.LoadScene("Tutorial 3", LoadSceneMode.Single);
    }
}
