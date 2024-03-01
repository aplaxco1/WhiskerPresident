using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor.SearchService;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSequenceOne : MonoBehaviour {
    public enum TutorialStep { clickBills, clickPaper, putPaperDown, clickBin, finish}
    [SerializeField] private GameObject stackOfBills;
    private GameObject paper;
    [SerializeField] private GameObject bin;
    [SerializeField] private SimpleBillMovement billMovementScript;
    [SerializeField] private GameObject completeButton;
    public static TutorialStep currentStep;

    // Start is called before the first frame update
    void Start()
    {
        currentStep = TutorialStep.clickBills;
    }

    // Update is called once per frame
    void Update()
    {
        bool billHighlighted = false;
        bool paperHighlighted = false;
        bool binHighlighted = false;
        switch (currentStep)
        {
            case TutorialStep.clickBills:
                Debug.Log("Click the Bill!");
                billHighlighted = true;
                break;
            case TutorialStep.clickPaper:
                Debug.Log("Click the Paper!");
                paperHighlighted = true;
                break;
            case TutorialStep.putPaperDown:
                Debug.Log("Put the Paper Down!");
                paperHighlighted = true;
                break;
            case TutorialStep.clickBin:
                Debug.Log("Click the Bin!");
                binHighlighted = true;
                break;
            case TutorialStep.finish:
                Debug.Log("Finished!");
                if(completeButton.activeSelf == false) {
                    completeButton.SetActive(true);
                }
                break;

        }
        HighlightObject(stackOfBills, billHighlighted);
        if (billMovementScript.billOut) {
            paper = billMovementScript.currBill.GetComponentInChildren<Renderer>().gameObject;
            HighlightObject(paper, paperHighlighted);
        }
        HighlightObject(bin, binHighlighted);
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
        SceneManager.LoadScene("Tutorial 2", LoadSceneMode.Single);
    }
}
