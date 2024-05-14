using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BillReviewController : MonoBehaviour
{
    public static BillReviewController Instance;

    public GameObject reviewTextPrefab;
    
    public float billStartX;
    public float billStartY;
    public float billStartZ;

    public float billXGap;
    public float billYGap;
    public float billZGap;

    private int index;
    private int numBills;
    private GameObject activeBill;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //ReviewBills();
        index = 0;
        numBills = 0;
        if (BillContentsManager.Instance) {numBills = BillContentsManager.Instance.savedBills.childCount;}
        if (numBills > 0)
        {
            activateBill(0);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            prevBill();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            nextBill();
        }
    }
    public void prevBill()
    {
        if (index > 0) {
            index = index - 1;
            activeBill.SetActive(false); //disable current bill
            activateBill(index);
        }
    }
    public void nextBill()
    {
        if (index < numBills - 1) {
            index = index + 1;
            activeBill.SetActive(false); //disable current bill
            activateBill(index);
        }
    }

    public void activateBill(int n) {
        activeBill = BillContentsManager.Instance.savedBills.GetChild(n).gameObject;
        activeBill.SetActive(true);
        activeBill.transform.position = new Vector3(0,0.479f,0);
        activeBill.transform.eulerAngles = new Vector3(90,0,0);
        activeBill.transform.localScale = new Vector3(4,4,4);
    }

    
    public void ReviewBills()
    {
        Vector3 billPos = new Vector3(billStartX, billStartY, billStartZ);
        Vector3 billRot = new Vector3(180, 0, 180);
        if (BillContentsManager.Instance == null)
        {
            print("Warning: No bills saved. Did you start the BillReview scene independently?");
            return;
        }
        Transform savedBills = BillContentsManager.Instance.savedBills;
        foreach (Transform t in savedBills)
        {
            GameObject b = t.gameObject;
            //print(b.name);
            b.SetActive(true);
            b.transform.position = billPos;
            b.transform.eulerAngles = billRot;
            billPos += new Vector3(billXGap, billYGap, billZGap);
            
            BillController controller = b.GetComponent<BillController>();
            float passed = controller.evaluatePassVeto();
            StatVector statVector = controller.CalculateOutcome();

            GameObject textObject = Instantiate(reviewTextPrefab, t, false);
            string passedString;
            if (passed == 0)
            {
                passedString = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "ignored") + "\n";
            } 
            else if (passed > 0)
            {
                passedString = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "passed") + "\n";
            }
            else
            {
                passedString = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "vetoed") + "\n";
            }
            textObject.GetComponentInChildren<TMP_Text>().text = "" + passedString + "\n" + statVector.StringConversion();
        }
        BillReviewCameraManager.Instance.SetLastBill(billPos.x);
    }
}
