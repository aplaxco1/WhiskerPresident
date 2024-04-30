using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBillMovement : MonoBehaviour
{
    [Header("BillVariables")]
    public GameObject billPrefab;
    [Header("AdjustableBillPositions")] 
    public GameObject stack;
    private GameObject currBill;
    private Vector3 stackPosition;
    // Start is called before the first frame update
    void Start()
    {
        stackPosition = new Vector3(stack.transform.position.x, stack.transform.position.y+0.095f, stack.transform.position.z);
        createBill();
    }

    // Update is called once per frame
    void Update()
    {
        if (currBill.transform.position.y != stackPosition.y) { // THIS IS BAD! IT CHECKS EVERY FRAME
            createBill();
        }
    }
    void createBill() {
        currBill = Instantiate(billPrefab, stackPosition, Quaternion.identity);
        currBill.GetComponent<BillController>().InitializeBill();
    }
}
