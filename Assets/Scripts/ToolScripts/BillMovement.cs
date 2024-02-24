using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillMovement : ToolClass
{

    [Header("BillVariables")]
    [SerializeField]
    public GameObject billPrefab;
    [SerializeField]
    public float speed = 5f;
    
    [Header("AdjustableBillPositions")]
    public Vector3 billPosition = new Vector3(0f, 0.83f, 0.08f);
    public Vector3 stackPosition = new Vector3(-0.57f, 0.91f, -0.16f);
    public Vector3 organizerPosition = new Vector3(0.57f, 0.87f, -0.16f);

    private bool billOut = false;
    private GameObject currBill;
    private bool billMoving = false;
    
    void Start()
    {
    }

    void Update()
    {
        // if the player right clicks, check where they clicked
        if (Input.GetMouseButtonDown(0) && !billMoving && isActive) {
            // create ray from camera to mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject.CompareTag("Stack") && !billOut && !currBill) {
                    currBill = Instantiate(billPrefab, stackPosition, Quaternion.identity);
                    // add outline to currBill HERE
                    currBill.GetComponentInChildren<Renderer>().material.SetFloat("_Highlighted", 1);
                    StartCoroutine(moveBill(billPosition, false));
                }
                else if (hit.collider.gameObject.CompareTag("Organizer") && billOut) {
                    StartCoroutine(moveBill(organizerPosition, true));
                }
                else if (hit.collider.gameObject.CompareTag("Bill")) {
                    // display bill on screen
                    Debug.Log("DISPLAYING BILL DETAILS");
                }
            }

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
            Debug.Log(currBill.GetComponentInChildren<BillController>().evaluatePassVeto());
            if (currBill.GetComponentInChildren<BillController>().evaluatePassVeto() == 0) {Timer.timeValue -= 10;}
            Destroy(currBill);
            billOut = false;
        }
        else {
            billOut = true;
        }

        billMoving = false;
    }

    public void addObjectHighlighting() {
        GameObject stack = GameObject.FindGameObjectWithTag("Stack");
        // add outline to stack.transform.parent.gameObject HERE
        stack.GetComponentInParent<Renderer>().material.SetFloat("_Highlighted", 1);
        if (billOut) {
            // add outline to currBill HERE
            currBill.GetComponentInChildren<Renderer>().material.SetFloat("_Highlighted", 1);
        }
        GameObject organizer = GameObject.FindGameObjectWithTag("Organizer");
        // add outline to organizer.transform.parent.gameObject HERE
        organizer.GetComponentInParent<Renderer>().material.SetFloat("_Highlighted", 1);
    }

    public void removeObjectHighlighting() {
        GameObject stack = GameObject.FindGameObjectWithTag("Stack");
        // remove outline from stack.transform.parent.gameObject HERE
        stack.GetComponentInParent<Renderer>().material.SetFloat("_Highlighted", 0);
        if (billOut) {
            // remove outline from currBill HERE
            currBill.GetComponentInParent<Renderer>().material.SetFloat("_Highlighted", 0);
        }
        GameObject organizer = GameObject.FindGameObjectWithTag("Organizer");
        // remove outline from organizer.transform.parent.gameObject HERE
        organizer.GetComponentInParent<Renderer>().material.SetFloat("_Highlighted", 0);
    }
}