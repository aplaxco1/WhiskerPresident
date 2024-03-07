using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialBillMovement : ToolClass
{

    [Header("BillVariables")]
    [SerializeField]
    public GameObject billPrefab;
    [SerializeField]
    public float speed;
    public float rotateSpeed;

    [Header("AdjustableBillPositions")] 
    public Vector3 billPosition;
    public Vector3 stackPosition;
    public Vector3 organizerPosition;
    public Vector3 inspectingPosition;

    public Vector3 flatRotation = new(0, 0, 0); // THIS USED TO BE A FLOAT (0) - Autumn
    public Vector3 inspectRotation = new(80, 180, 0); // THIS USED TO BE A FLOAT (280) - Autumn
        
    public bool billOut;
    public GameObject currBill;
    private bool billMoving;
    public bool inspectingBill;
    private bool billRotating;
    
    void Start()
    {
    }

    void Update()
    {
        // if the player right clicks, check where they clicked
        if (Input.GetMouseButtonDown(0) && !billMoving && !billRotating && isActive) {
            // create ray from camera to mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject.CompareTag("Stack") && !billOut && !currBill && !inspectingBill)
                {
                    TutorialSequenceOne.NextStepInTutorial(1);
                    MoveBillToTable(hit);
                }
                else if (hit.collider.gameObject.CompareTag("Organizer") && billOut && !inspectingBill)
                {
                    TutorialSequenceOne.NextStepInTutorial(4);
                    MoveBillToFinished(hit);
                }
                else if (hit.collider.gameObject.CompareTag("Bill") && !inspectingBill)
                {
                    TutorialSequenceOne.NextStepInTutorial(2);
                    TutorialSequenceThree.NextStepInTutorial(3);
                    // display bill on screen
                    InspectBill();
                } 
                else if (hit.collider.gameObject.CompareTag("Bill") && inspectingBill) 
                {
                    TutorialSequenceOne.NextStepInTutorial(3);
                    UninspectBill();
                }
            }
        }
    }

    private void MoveBillToTable(RaycastHit hit) 
    {
        currBill = Instantiate(billPrefab, stackPosition, Quaternion.identity);
        ToggleHighlights(currBill.GetComponentInChildren<Renderer>(), 1);
        ToggleHighlights(hit.collider.gameObject.GetComponentInParent<Renderer>(), 0);
        GameObject organizer = GameObject.FindGameObjectWithTag("Organizer");
        ToggleHighlights(organizer.GetComponentInParent<Renderer>(), 1);
        StartCoroutine(MoveBill(billPosition, false));
    }

    private void MoveBillToFinished(RaycastHit hit)
    {
        ToggleHighlights(currBill.GetComponentInChildren<Renderer>(), 0);
        ToggleHighlights(hit.collider.gameObject.GetComponentInParent<Renderer>(), 0);
        GameObject stack = GameObject.FindGameObjectWithTag("Stack");
        ToggleHighlights(stack.GetComponentInParent<Renderer>(), 1);
        StartCoroutine(MoveBill(organizerPosition, true));
    }

    private void InspectBill()
    {
        inspectingBill = true;
        GameObject organizer = GameObject.FindGameObjectWithTag("Organizer");
        ToggleHighlights(organizer.GetComponentInParent<Renderer>(), 0);
        StartCoroutine(InspectBillMovememt());
    }
    
    private void UninspectBill()
    {
        inspectingBill = false;
        GameObject organizer = GameObject.FindGameObjectWithTag("Organizer");
        ToggleHighlights(organizer.GetComponentInParent<Renderer>(), 1);
        StartCoroutine(UninspectBillMovement());
    }

    private IEnumerator InspectBillMovememt()
    {
        StartCoroutine(MoveBill(inspectingPosition, false));
        billRotating = true;
        while (true)
        {
            currBill.transform.rotation = Quaternion.RotateTowards(currBill.transform.rotation, Quaternion.Euler(inspectRotation), rotateSpeed * Time.deltaTime);
            if (currBill.transform.rotation == Quaternion.Euler(inspectRotation))
            {
                yield break;
            }
            // IDK WHAT ALL THIS IS IM TOO STUPID SORRY - Autumn
            // currBill.transform.Rotate(Vector3.left, rotateSpeed * Time.deltaTime);
            // //print(currBill.transform.eulerAngles.x );
            // if (Mathf.Abs((currBill.transform.eulerAngles.x + inspectRotation)) < 2f)
            // {
            //     currBill.transform.eulerAngles = new Vector3(inspectRotation, 0, 0);
            //     yield break;
            // }
            yield return null;
            billRotating = false;
        }
    }
    
    private IEnumerator UninspectBillMovement()
    {
        StartCoroutine(MoveBill(billPosition, false));
        billRotating = true;
        while (true)
        {
            currBill.transform.rotation = Quaternion.RotateTowards(currBill.transform.rotation, Quaternion.Euler(0, 0, 0), rotateSpeed * Time.deltaTime);
            if (currBill.transform.rotation == Quaternion.Euler(0, 0, 0)) {
                yield break;
            }
            // IDK WHAT ALL THIS IS IM TOO STUPID - Autumn
            // currBill.transform.Rotate(Vector3.right, rotateSpeed * Time.deltaTime);
            // //print(currBill.transform.eulerAngles.x);
            // if (Mathf.Abs((currBill.transform.eulerAngles.x - flatRotation)) < 2f)
            // {
            //     currBill.transform.eulerAngles = new Vector3(flatRotation, 0, 0);
            //     yield break;
            // }
            yield return null;
            billRotating = false;
        }
    }

    IEnumerator MoveBill(Vector3 target, bool destroy) 
    {
        billMoving = true;

        while (Vector3.Distance(currBill.transform.position, target) > 0f) {
            currBill.transform.position = Vector3.MoveTowards(currBill.transform.position, target, Time.deltaTime * speed);
            yield return null;
        }

        if (destroy)
        {
            // give bill status to Tutorial
            if ((int)TutorialSequenceThree.currentStep < 2) {
                TutorialSequenceThree.Instance.GiveBillStatus(currBill.GetComponent<BlankBillController>().evaluatePassVeto());
            }
            else {
                TutorialSequenceThree.NextStepInTutorial(4);
            }
            //Debug.Log("bill status: " + currBill.GetComponent<BlankBillController>().evaluatePassVeto());
            Destroy(currBill);
            billOut = false;
        }
        else
        {
            billOut = true;
        }

        billMoving = false;
    }

    public void AddObjectHighlighting()
    {
        if (billOut)
        {
            ToggleHighlights(currBill.GetComponentInChildren<Renderer>(), 1);
            GameObject organizer = GameObject.FindGameObjectWithTag("Organizer");
            ToggleHighlights(organizer.GetComponentInParent<Renderer>(), 1);
        }
        else
        {
            GameObject stack = GameObject.FindGameObjectWithTag("Stack");
            ToggleHighlights(stack.GetComponentInParent<Renderer>(), 1);
        }
    }

    public void RemoveObjectHighlighting()
    {
        GameObject stack = GameObject.FindGameObjectWithTag("Stack");
        ToggleHighlights(stack.GetComponentInParent<Renderer>(), 0);
        if (billOut)
        {
            ToggleHighlights(currBill.GetComponentInChildren<Renderer>(), 0);
        }
        GameObject organizer = GameObject.FindGameObjectWithTag("Organizer");
        ToggleHighlights(organizer.GetComponentInParent<Renderer>(), 0);
    }
    private void ToggleHighlights(Renderer renderer, int n)
    {
        foreach (Material material in renderer.materials)
        {
            material.SetFloat("_Highlighted", Mathf.Clamp(n, 0, 1));
        }
    }
}