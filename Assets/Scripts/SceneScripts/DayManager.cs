using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DayManager : MonoBehaviour
{
    public DayInfo dayInfo;
    
    public static DayManager Instance;
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
        dayInfo = new DayInfo();
        SaveManager.Instance.LoadFromFile(1);

        if (BillContentsManager.Instance != null)
        {
            BillContentsManager.Instance.WipeSavedBills();
        }

        if (SaveManager.Instance.currentSaveData == null || SaveManager.Instance.currentSaveData.dayInfo.day == 0)
        {
            InitializeDefaultStats();

        }
    }

    void InitializeDefaultStats()
    { 
        // print("init default stats");
        dayInfo.statA = 50;
        dayInfo.statB = 50;
        dayInfo.statC = 50;

        dayInfo.sinkA = -10;
        dayInfo.sinkB = -10;
        dayInfo.sinkC = -10;

        dayInfo.day = 1;
        dayInfo.impeached = false;
    }

}
