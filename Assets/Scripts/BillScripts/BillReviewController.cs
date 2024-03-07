using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillReviewController : MonoBehaviour
{
    public static BillReviewController Instance;
    
    public float billStartX;
    public float billStartY;
    public float billStartZ;

    public float billXGap;
    public float billYGap;
    public float billZGap;

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
        ReviewBills();
    }

    
    public void ReviewBills()
    {
        Vector3 billPos = new Vector3(billStartX, billStartY, billStartZ);
        Vector3 billRot = new Vector3(180, 0, 180);
        Transform savedBills = BillContentsManager.Instance.savedBills;
        foreach (Transform t in savedBills)
        {
            GameObject b = t.gameObject;
            //print(b.name);
            b.SetActive(true);
            b.transform.position = billPos;
            b.transform.eulerAngles = billRot;
            billPos += new Vector3(billXGap, billYGap, billZGap);
        }
    }
}
