using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BillContentsManager : MonoBehaviour
{
    public static BillContentsManager Instance;
    
    [Tooltip("A = Generate symbol 1 or 2, \n B = Generate symbol 1 or 2 or 3, \n M = Generate modifier")]
    public string templateSequence;
    public List<List<BillController.SymbolType>> billContentLists;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
        
        if (billContentLists == null)
        {
            billContentLists = new List<List<BillController.SymbolType>>();
        }
    }

    public void SaveBill(List<BillController.SymbolType> bill)
    {
        if (bill == null || bill.Count == 0)
        {
            print("WARNING: attempted to save null/empty bill");
            return;
        }
        billContentLists.Add(bill);
        // string s = "";
        // foreach (List<BillController.SymbolType> b in billContentLists)
        // {
        //     s += "bill of length " + b.Count + "\n";
        // }
        // print(s);
    }

    public void WipeSavedBills()
    {
        billContentLists = new List<List<BillController.SymbolType>>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {;
            SceneManager.LoadScene("BillReview");
        }
    }
}
