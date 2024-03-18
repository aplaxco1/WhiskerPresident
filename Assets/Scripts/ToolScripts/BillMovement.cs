using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

    public Vector3 flatRotation = new Vector3(0, 0, 0); // THIS USED TO BE A FLOAT (0) - Autumn
    public Vector3 inspectRotation = new Vector3(80, 180, 0); // THIS USED TO BE A FLOAT (280) - Autumn
        
    private bool billOut;
    private GameObject currBill;
    private bool billMoving;
    public bool inspectingBill;
    private bool billRotating;

    //Tracks time held down
    private float holdStartTime;

    public float holdDuration = 1f;

    //laserStuff (i.e. Abe's bullshit)
    [Header("Adjustable Laser Variables")]
    [SerializeField]
    public float laserWidth = 0.01f;
    [SerializeField]
    public Vector3 laserOffset = new Vector3(0.0f, -0.45f, 0.0f);
    [SerializeField]
    public GameObject laserDot;
    public Material laserMaterial;

    [Header("Global Laser Variables")]
    // variables to capture location on desk for cats attention
    [SerializeField]
    public bool isOnDesk = false;
    [SerializeField]
    public Vector3 laserDeskLocation = new Vector3(0f, 0f, 0f);
    public BillMovement billScript;
    public LayerMask billLayer;

    private GameObject lineObj;
    private LineRenderer lineRender;

    private bool billActive = false;

    //public Collider BillCollider;
    public Collider trayCollider;

    void Start()
    {
        //stuff for bill move sorry 
        //Sincerely Abraham
        lineObj = new GameObject("LaserPointerLine");
        lineRender = lineObj.AddComponent<LineRenderer>();
        lineRender.material = laserMaterial ? laserMaterial : new Material(Shader.Find("Sprites/Default"));
        lineRender.widthMultiplier = laserWidth;
        lineRender.positionCount = 2;
        lineRender.startColor = Color.red;
        lineRender.endColor = Color.red;
        lineObj.SetActive(true);
    }

    void Update()
    {
        

        if (Input.GetMouseButton(1) && !billMoving && !billRotating && isActive)
        {
            dragBill();
            /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Bill") && !inspectingBill)
                {

                    dragBill();
                }
            }*/
        }


        
        if (Input.GetMouseButtonDown(0) && !billMoving && !billRotating && isActive) {
            // create ray from camera to mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //if mouse is held down it tracks the time. 
            if (Input.GetMouseButtonDown(0))
            {
                holdStartTime = Time.time;
            }

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
        holdStartTime = 0f;
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
        StartCoroutine(inspectBillMovememt());
    }
    
    private void uninspectBill()
    {
        inspectingBill = false;
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

    IEnumerator moveBill(Vector3 target, bool destroy) 
    {
        billMoving = true;

        while (Vector3.Distance(currBill.transform.position, target) > 0f) {
            currBill.transform.position = Vector3.MoveTowards(currBill.transform.position, target, Time.deltaTime * speed);
            yield return null;
        }

        if (destroy) {
            //Debug.Log(currBill.GetComponentInChildren<BillController>().evaluatePassVeto());
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
        GameObject myObject = GameObject.Find("Bill(Clone)");
        if (myObject != null)
        {
            // Do something with myObject
            //Debug.Log("test");
            billActive = true;
            laserDot = myObject;
            laserDot.SetActive(true);
            laserDot.SetActive(isOnDesk);
        }





        if (isActive && billActive)
        //if(true)
        {
            // set laser dot active if laser is on desk
            laserDot.SetActive(isOnDesk);
            lineObj.SetActive(true);
            // create ray from camera to mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // calculate position of visual laser
            Vector3 laserStartPos = new Vector3(Camera.main.transform.position.x + laserOffset.x, Camera.main.transform.position.y + laserOffset.y, Camera.main.transform.position.z + laserOffset.z);

            // determine raycast collision
            RaycastHit mouseHit;
            //hitInfo = mouseHit;
            if (Physics.Raycast(ray, out mouseHit))
            {
                //Debug.Log("hit");
                //drawLine(laserStartPos, mouseHit.point);
                Vector3 direction = mouseHit.point - laserStartPos;
                RaycastHit laserHit;

                
                // make sure actual visual laser doesnt go through any objects
                if (Physics.Raycast(laserStartPos, direction, out laserHit, Mathf.Infinity) && !isOnDesk)
                {
                    //drawLine(laserStartPos, laserHit.point);
                }
                // check if on desk
                if (mouseHit.collider.gameObject.layer == LayerMask.NameToLayer("Desk"))
                {
                    if (mouseHit.collider.gameObject.CompareTag("Bill") && !inspectingBill)
                    {
                        isOnDesk = false;
                        laserDot.SetActive(true);
                    }
                    else
                    {
                        isOnDesk = true;
                        laserDot.SetActive(true);
                    }
                    laserDeskLocation = mouseHit.point;
                    laserDot.transform.position = mouseHit.point;
                    laserDot.transform.rotation = Quaternion.FromToRotation(laserDot.transform.up, mouseHit.normal) * laserDot.transform.rotation;
                }
                //Collider currBillBounds = currBill.GetComponent<Renderer>().bounds;
                //Collider trayBounds = tray.GetComponent<Renderer>().bounds;

                bool overlap = currBill.GetComponentInChildren<Collider>().bounds.Intersects(trayCollider.bounds);

                if (overlap)
                {
                    moveBillToFinished(mouseHit);
                }
                else
                {
                    //isOnDesk = false;
                }
            }
            else
            {
                float distance = 100f;
                //drawLine(laserStartPos, ray.direction * distance);
                isOnDesk = false;
            }
        }

    }
}