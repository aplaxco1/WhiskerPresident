using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class BillContentsManager : MonoBehaviour
{
    public static BillContentsManager Instance;
    
    [Tooltip("A = Generate value 1 or 2, \n B = Generate value 1 or 2 or 3, \n M = Generate modifier")]
    public string templateSequence;
    public Transform savedBills;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);

        savedBills = transform.Find("SavedBills");
        //WipeSavedBills();
    }

    public void SaveBill(GameObject bill)
    {
        if (bill == null)
        {
            print("WARNING: attempted to save null/empty bill");
            return;
        }

        GameObject savedBill = Instantiate(bill, Vector3.zero, Quaternion.identity, savedBills);
        savedBill.SetActive(false);
    }

    public void WipeSavedBills()
    {
        foreach(Transform child in savedBills)
        {
            Destroy(child.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {;
            SceneManager.LoadScene("BillReview");
        }
    }
}