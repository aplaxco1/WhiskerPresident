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
        SaveManager.Instance.LoadFromFile(1);

        if (BillContentsManager.Instance != null)
        {
            BillContentsManager.Instance.WipeSavedBills();
        }

        if (SaveManager.Instance.currentSaveData == null)
        {
            InitializeStats();

        }
    }

    void InitializeStats()
    {
        StatVector startingStats = new StatVector
        {
            StatA = 50,
            StatB = 50,
            StatC = 50
        };
        AdjustStats(startingStats);

        dayInfo.sinkA = -10;
        dayInfo.sinkB = -10;
        dayInfo.sinkC = -10;
    }

}
