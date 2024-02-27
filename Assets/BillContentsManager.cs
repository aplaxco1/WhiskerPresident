using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    }

    public void SaveBill(List<BillController.SymbolType> bill)
    {
        billContentLists.Append(bill);
    }

    public void WipeSavedBills()
    {
        billContentLists = new List<List<BillController.SymbolType>>();
    }

}
