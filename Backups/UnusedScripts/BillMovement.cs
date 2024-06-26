using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class BillMovement : ToolClass
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

    public Vector3 flatRotation = new Vector3(0, 0, 0);
    public Vector3 inspectRotation = new Vector3(80, 180, 0);
        
    [Header("BillStatusVariables")]
    private bool billOut;
    private GameObject currBill;
    private bool billMoving;
    public bool inspectingBill;
    private bool billRotating;
    private bool draggingBill = false;
    public Texture2D draggingCursorTexture;

    private int LayerIgnoreRaycast;

    void Start()
    {
        LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(1) && isActive && billOut && !inspectingBill && !billMoving && !billRotating)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("Bill") && !draggingBill) {
                draggingBill = true;
                Cursor.SetCursor(draggingCursorTexture, Vector2.zero, CursorMode.Auto);
                changeObjectLayer(currBill, LayerIgnoreRaycast);
            }
            else {
                stopDragging();
            }
        }


        if (draggingBill && isActive) {
            dragBill();
        }
        else if (draggingBill && !isActive) {
            stopDragging();
        }

        
        if (Input.GetMouseButtonDown(0) && !billMoving && !billRotating && isActive && !draggingBill) {
            // create ray from camera to mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject.CompareTag("Stack") && !billOut && !currBill && !inspectingBill) {
                    moveBillToTable(hit);
                }
                else if (hit.collider.gameObject.CompareTag("Organizer") && billOut && !inspectingBill) {
                    moveBillToFinished(hit);
                }
                else if (hit.collider.gameObject.CompareTag("Bill") && !inspectingBill) {
                    // display bill on screen
                    inspectBill();
                } 
                else if (inspectingBill) {
                    uninspectBill();
                }
            }
        }
    }

    private void moveBillToTable(RaycastHit hit)
    {
        currBill = Instantiate(billPrefab, stackPosition, Quaternion.identity);
        currBill.GetComponent<BillController>().InitializeBill();
        toggleHighlights(currBill.GetComponentInChildren<Renderer>(), 1);
        toggleHighlights(hit.collider.gameObject.GetComponentInParent<Renderer>(), 0);
        GameObject organizer = GameObject.FindGameObjectWithTag("Organizer");
        toggleHighlights(organizer.GetComponentInParent<Renderer>(), 1);
        StartCoroutine(moveBill(billPosition, false));
    }

    private void moveBillToFinished(RaycastHit hit)
    {
        toggleHighlights(currBill.GetComponentInChildren<Renderer>(), 0);
        toggleHighlights(hit.collider.gameObject.GetComponentInParent<Renderer>(), 0);
        GameObject stack = GameObject.FindGameObjectWithTag("Stack");
        toggleHighlights(stack.GetComponentInParent<Renderer>(), 1);
        StartCoroutine(moveBill(organizerPosition, true));
    }

    private void inspectBill()
    {
        inspectingBill = true;
        GameObject organizer = GameObject.FindGameObjectWithTag("Organizer");
        toggleHighlights(organizer.GetComponentInParent<Renderer>(), 0);
        StartCoroutine(inspectBillMovememt());
    }
    
    private void uninspectBill()
    {
        inspectingBill = false;
        GameObject organizer = GameObject.FindGameObjectWithTag("Organizer");
        toggleHighlights(organizer.GetComponentInParent<Renderer>(), 1);
        StartCoroutine(uninspectBillMovement());
    }

    private IEnumerator inspectBillMovememt()
    {
        StartCoroutine(moveBill(inspectingPosition, false));
        billRotating = true;
        while (true)
        {
            currBill.transform.rotation = Quaternion.RotateTowards(currBill.transform.rotation, Quaternion.Euler(inspectRotation), rotateSpeed * Time.deltaTime);
            if (currBill.transform.rotation == Quaternion.Euler(inspectRotation)) {
                yield break;
            }
            yield return null;
            billRotating = false;
        }
    }
    
    private IEnumerator uninspectBillMovement()
    {
        StartCoroutine(moveBill(billPosition, false));
        billRotating = true;
        while (true)
        {
            currBill.transform.rotation = Quaternion.RotateTowards(currBill.transform.rotation, Quaternion.Euler(0, 0, 0), rotateSpeed * Time.deltaTime);
            if (currBill.transform.rotation == Quaternion.Euler(0, 0, 0)) {
                yield break;
            }
            yield return null;
            billRotating = false;
        }
    }

    IEnumerator moveBill(Vector3 target, bool destroy) 
    {
        billMoving = true;

        while (Vector3.Distance(currBill.transform.position, target) > 0f) {
            currBill.transform.position = Vector3.MoveTowards(currBill.transform.position, target, Time.deltaTime * speed);
            yield return null;
        }

        if (destroy) {
            if (currBill.GetComponentInChildren<BillController>().evaluatePassVeto() == 0) {Timer.timeValue -= 10;}
            currBill.GetComponentInChildren<BillController>().UninitializeBill();
            billOut = false;
        }
        else {
            billOut = true;
        }

        billMoving = false;
    }

    public void addObjectHighlighting() {
        if (billOut) {
            toggleHighlights(currBill.GetComponentInChildren<Renderer>(), 1);
            GameObject organizer = GameObject.FindGameObjectWithTag("Organizer");
            toggleHighlights(organizer.GetComponentInParent<Renderer>(), 1);
        } else {
            GameObject stack = GameObject.FindGameObjectWithTag("Stack");
            toggleHighlights(stack.GetComponentInParent<Renderer>(), 1);
        }
    }

    public void removeObjectHighlighting() {
        GameObject stack = GameObject.FindGameObjectWithTag("Stack");
        toggleHighlights(stack.GetComponentInParent<Renderer>(), 0);
        if (billOut) {
            toggleHighlights(currBill.GetComponentInChildren<Renderer>(), 0);
        }
        GameObject organizer = GameObject.FindGameObjectWithTag("Organizer");
        toggleHighlights(organizer.GetComponentInParent<Renderer>(), 0);
    }

    private void toggleHighlights(Renderer renderer, int n) {
        foreach (Material material in renderer.materials) {
            material.SetFloat("_Highlighted", Mathf.Clamp(n, 0, 1));
        }
    }

    public void dragBill()
    {

        // create ray from camera to mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Desk")) {
                currBill.transform.position = hit.point;
            }
            else {
                currBill.transform.position = billPosition;
            }

            if (hit.collider.gameObject.CompareTag("Organizer")) {
                moveBillToFinished(hit);
                draggingBill = false;
                Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
            }
        }
    }

    private void stopDragging() {
        draggingBill = false;
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        changeObjectLayer(currBill, LayerMask.NameToLayer("Desk"));
        StartCoroutine(moveBill(billPosition, false));
    }

    private void changeObjectLayer(GameObject obj, int layer) {
        obj.layer = layer;
        foreach (Transform child in obj.transform) {
            changeObjectLayer(child.gameObject, layer);
        }
    }
}