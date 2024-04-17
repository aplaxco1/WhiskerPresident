using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSequence : MonoBehaviour {
    public static TutorialSequence Instance { get; private set; }
    public enum TutorialStep {getBill, readBill, switchLaser, stampBill, submitBill, finish}
    [SerializeField] private GameObject step1;
    [SerializeField] private GameObject step2;
    [SerializeField] private GameObject step3;
    [SerializeField] private GameObject step4;
    [SerializeField] private GameObject step5;
    [SerializeField] private GameObject completeButton;
    [SerializeField] private TutorialBillMovement billMovementScript;
    //[SerializeField] private GameObject blankBillPreset;
    //[SerializeField] private GameObject regularBillPreset;

    // Objects that need highlighting
    //[SerializeField] private GameObject acceptInkpad;
    //[SerializeField] private GameObject rejectInkpad;

    public static TutorialStep currentStep;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //bill.GetComponent<BillController>().isBlank = true;
        //retryMessage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentStep)
        {
            case TutorialStep.getBill:
                step1.SetActive(true);
                break;
            case TutorialStep.readBill:
                step1.SetActive(false);
                step2.SetActive(true);
                break;
            case TutorialStep.switchLaser:
                step2.SetActive(false);
                step3.SetActive(true);
                break;
            case TutorialStep.stampBill:
                step3.SetActive(false);
                step4.SetActive(true);
                break;
            case TutorialStep.submitBill:
                step4.SetActive(false);
                step5.SetActive(true);
                break;
            case TutorialStep.finish:
                step5.SetActive(false);
                completeButton.SetActive(true);
                break;
            default:
                Debug.LogWarning("Somehow reading impossible tutorial step, send help");
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
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
