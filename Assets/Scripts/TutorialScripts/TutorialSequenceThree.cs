using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor.SearchService;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSequenceThree : MonoBehaviour {
    public enum TutorialStep {approveInk, stampApprove, rejectInk, stampReject, readBill, stampBill, finish}
    [SerializeField] private GameObject step1;
    [SerializeField] private GameObject step2;
    [SerializeField] private GameObject step3;
    [SerializeField] private GameObject step4;
    [SerializeField] private GameObject completeButton;

    [SerializeField] private GameObject bill;

    // Objects that need highlighting
    [SerializeField] private GameObject acceptInkpad;
    [SerializeField] private GameObject rejectInkpad;

    public static TutorialStep currentStep;

    // Start is called before the first frame update
    void Start()
    {
        //bill.GetComponent<BillController>().isBlank = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentStep)
        {
            case TutorialStep.approveInk:
                step1.SetActive(true);
                HighlightObject(acceptInkpad, true);
                break;
            case TutorialStep.stampApprove: // make sure to let the player know the status of the paper
                HighlightObject(acceptInkpad, false);
                break;
            case TutorialStep.rejectInk:
                step1.SetActive(false);
                step2.SetActive(true);
                HighlightObject(rejectInkpad, true);
                break;
            case TutorialStep.stampReject:
                HighlightObject(rejectInkpad, false);
                break;
            case TutorialStep.readBill:
                step2.SetActive(false);
                step3.SetActive(true);
                break;
            case TutorialStep.stampBill:
                step3.SetActive(false);
                step4.SetActive(true);
                break;
            case TutorialStep.finish:
                step4.SetActive(false);
                Debug.Log("Finished!");
                if(completeButton.activeSelf == false) {
                    completeButton.SetActive(true);
                }
                break;

        }
        // HighlightObject(stackOfBills, billHighlighted);
       // if (billMovementScript.billOut) {
       //     paper = billMovementScript.currBill.GetComponentInChildren<Renderer>().gameObject;
       //     HighlightObject(paper, paperHighlighted);
       // }
       // HighlightObject(bin, binHighlighted);
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
            Debug.Log("Moved on to Step: " + currentStep);
            return true;
        }
        else
        {
            return false;
        }
    }


    public void NextTutorial() {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
