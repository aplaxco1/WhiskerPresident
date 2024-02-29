using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor.VersionControl;
using UnityEngine;

public class TutorialSequenceOne : MonoBehaviour {
    public enum TutorialStep { clickBills, clickPaper, clickBin }
    [SerializeField] private GameObject stackOfBills;
    [SerializeField] private GameObject paper;
    [SerializeField] private GameObject bin;
    [SerializeField] private SimpleBillMovement billMovementScript;
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
            case TutorialStep.clickBin:
                Debug.Log("Click the Bin!");
                binHighlighted = true;
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
}
