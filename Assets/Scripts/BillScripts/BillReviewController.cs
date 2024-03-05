using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillReviewController : MonoBehaviour
{
    public static BillReviewController Instance;
    public List<List<BillController.SymbolType>> savedBills;
    public GameObject billPrefab;
    
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
        if (BillContentsManager.Instance != null)
        {
            savedBills = BillContentsManager.Instance.billContentLists;
        }
        else
        {
            savedBills = new List<List<BillController.SymbolType>>();
            savedBills.Add(new List<BillController.SymbolType>('1'));
        }
        GenerateBills();
    }

    public List<BillController.SymbolType> GrabNextBill()
    {
        List<BillController.SymbolType> nextBill;
        nextBill = savedBills[0];
        savedBills.RemoveAt(0);
        return nextBill;
    }
    
    public void GenerateBills()
    {
        Vector3 billPos = new Vector3(billStartX, billStartY, billStartZ);
        Vector3 billRot = new Vector3(0, 0, 0);
        foreach (List<BillController.SymbolType> b in savedBills)
        {
            Instantiate(billPrefab, billPos, Quaternion.Euler(billRot));
            billPos += new Vector3(billXGap, billYGap, billZGap);
        }
    }
}
