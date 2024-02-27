using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class TutorialSequenceOne : MonoBehaviour {
    enum TutorialStep { clickBills, clickPaper, clickBin }
    [SerializeField] private GameObject stackOfBills;
    [SerializeField] private GameObject paper;
    [SerializeField] private GameObject bin;
    private TutorialStep currentStep;

    private bool isHighlighted;
    // Start is called before the first frame update
    void Start()
    {
        currentStep = TutorialStep.clickBills;
        isHighlighted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.)
        switch (currentStep)
        {
            case TutorialStep.clickBills:
                HighlightObject(gameObject);
                break;
        }
        
    }

    void HighlightObject(GameObject gameObject)
    {
        if (isHighlighted) {
            gameObject.GetComponent<Renderer>().material.SetFloat("_Highlighted", 1);
        }
        else {
            gameObject.GetComponent<Renderer>().material.SetFloat("_Highlighted", 0);
        }
    }
}
