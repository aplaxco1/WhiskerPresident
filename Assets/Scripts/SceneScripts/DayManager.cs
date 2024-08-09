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
        // SaveManager.Instance.LoadFromFile();

        if (BillContentsManager.Instance != null)
        {
            BillContentsManager.Instance.WipeSavedBills();
        }

        if (SaveManager.Instance.currentSaveData == null || SaveManager.Instance.currentSaveData.dayInfo.day == 0)
        {
            InitializeDefaultStats();
        }
        
        DayStart();
    }

    // NOTE: I don't think this is ever actually used.
    public void InitializeDefaultStats()
    { 
        print("init default stats");
        dayInfo.statA = 30;
        dayInfo.statB = 30;
        dayInfo.statC = 30;

        dayInfo.sinkA = -10;
        dayInfo.sinkB = -10;
        dayInfo.sinkC = -10;

        dayInfo.day = 1;
    }

}
