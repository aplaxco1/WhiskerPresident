using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillMovement : ToolClass
{

    [Header("BillVariables")]
    [SerializeField]
    public GameObject billPrefab;
    [SerializeField]
    public float speed = 0.01f;
    
    [Header("AdjustableBillPositions")]
    public Vector3 billPosition = new Vector3(0f, 0.83f, 0.08f);
    public Vector3 stackPosition = new Vector3(-0.57f, 0.91f, -0.16f);
    public Vector3 organizerPosition = new Vector3(0.57f, 0.87f, -0.16f);

    private bool billOut = false;
    private GameObject currBill;
    
    void Start()
    {
    }

    void Update()
    {
        // if the player right clicks, check where they clicked
        if (Input.GetMouseButtonDown(1) && isActive) {
            // create ray from camera to mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject.CompareTag("Stack") && !billOut) {
                    billOut = true;
                    currBill = Instantiate(billPrefab, stackPosition, Quaternion.identity);
                    StartCoroutine(moveBill(billPosition, false));
                }
                else if (hit.collider.gameObject.CompareTag("Organizer") && billOut) {
                    // remove bill (CHANGE THIS TO MOVE BILL TO ORGANIZER)
                    billOut = false;
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
        while (currBill.transform.position != target) {
            currBill.transform.position = Vector3.MoveTowards(currBill.transform.position, target, speed);
            yield return null;
        }

        if (destroy) {
            Destroy(currBill);
        }
    }
}