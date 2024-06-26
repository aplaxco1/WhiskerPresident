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

    //Tracks time held down
    private float holdStartTime;

    public float holdDuration = 1f;

    void Start()
    {
    }

    void Update()
    {
        // if the player right clicks, check where they clicked
        if (Input.GetMouseButtonDown(0) && !billMoving && !billRotating && isActive) {
            // create ray from camera to mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                holdStartTime = Time.time;
            }
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject.CompareTag("Stack") && !billOut && !currBill && !inspectingBill)
                {
                    MoveBillToTable(hit);
                }
                else if (hit.collider.gameObject.CompareTag("Organizer") && billOut && !inspectingBill)
                {
                    MoveBillToFinished(hit);
                }
                else if (hit.collider.gameObject.CompareTag("Bill") && !inspectingBill)
                {
                    // display bill on screen
                    InspectBill();
                } 
                else if (inspectingBill) 
                {
                    UninspectBill();
                }
            }
        }
        holdStartTime = 0f;
    }

    private void MoveBillToTable(RaycastHit hit) 
    {
        TutorialSequence.NextStepInTutorial(1);
        currBill = Instantiate(billPrefab, stackPosition, Quaternion.identity);
        currBill.GetComponent<BillController>().InitializeBill();
        ToggleHighlights(currBill.GetComponentInChildren<Renderer>(), 1);
        ToggleHighlights(hit.collider.gameObject.GetComponentInParent<Renderer>(), 0);
        GameObject organizer = GameObject.FindGameObjectWithTag("Organizer");
        ToggleHighlights(organizer.GetComponentInParent<Renderer>(), 1);
        StartCoroutine(MoveBill(billPosition, false));
    }

    private void MoveBillToFinished(RaycastHit hit)
    {
        TutorialSequence.NextStepInTutorial(5);
        ToggleHighlights(currBill.GetComponentInChildren<Renderer>(), 0);
        ToggleHighlights(hit.collider.gameObject.GetComponentInParent<Renderer>(), 0);
        GameObject stack = GameObject.FindGameObjectWithTag("Stack");
        ToggleHighlights(stack.GetComponentInParent<Renderer>(), 1);
        StartCoroutine(MoveBill(organizerPosition, true));
    }

    private void InspectBill()
    {
        TutorialSequence.NextStepInTutorial(2);
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
                //if (currBill.GetComponentInChildren<BillController>().evaluatePassVeto() == 0)
                //{
                //    Timer.timeValue -= 10;
                //}
                //Debug.Log("uh we hit uninitialize?");
                //currBill.GetComponentInChildren<BillController>().UninitializeBill()
                //;
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