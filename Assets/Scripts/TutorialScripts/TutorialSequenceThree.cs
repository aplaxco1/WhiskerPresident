using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSequenceThree : MonoBehaviour {
    public static TutorialSequenceThree Instance { get; private set; }
    public enum TutorialStep {stampApprove, stampReject, readBill, stampBill, finish}
    [SerializeField] private GameObject step1;
    [SerializeField] private GameObject step2;
    [SerializeField] private GameObject step3;
    [SerializeField] private GameObject step4;
    [SerializeField] private GameObject completeButton;
    [SerializeField] private GameObject retryMessage;
    [SerializeField] private float retryMessageTimer;
    [SerializeField] private TutorialBillMovement billMovementScript;
    [SerializeField] private GameObject blankBillPreset;
    [SerializeField] private GameObject regularBillPreset;

    private float billGoal;

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
        billMovementScript.billPrefab = blankBillPreset;
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
            case TutorialStep.stampApprove: // make sure to let the player know the status of the paper
                step1.SetActive(true);
                billGoal = 1;
                break;
            case TutorialStep.stampReject:
                step1.SetActive(false);
                step2.SetActive(true);
                billGoal = -1;
                break;
            case TutorialStep.readBill:
                billMovementScript.billPrefab = regularBillPreset;
                step2.SetActive(false);
                step3.SetActive(true);
                break;
            case TutorialStep.stampBill:
                step3.SetActive(false);
                step4.SetActive(true);
                break;
            case TutorialStep.finish:
                step4.SetActive(false);
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

    IEnumerator FlashRetry() {
        Debug.Log("Timer Called");
        retryMessage.SetActive(true);
        yield return new WaitForSeconds(retryMessageTimer);
        retryMessage.SetActive(false);
        Debug.Log("Timer Over");
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

    public void GiveBillStatus(float billEvaluation)
    {
        // if (billEvaluation != billGoal) {
        //     StartCoroutine(FlashRetry());
        // }
        if (billGoal == 1 && billEvaluation > 0) {
            NextStepInTutorial(1);
        }
        else if(billGoal == -1 && billEvaluation < 0) {
            NextStepInTutorial(2);
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
        SceneManager.LoadScene("Office", LoadSceneMode.Single);
    }
}
