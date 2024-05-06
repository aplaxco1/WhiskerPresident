using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DayManager : MonoBehaviour
{
    public int day;
    public bool impeached;
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
        StatVector startingStats = new StatVector();
        startingStats.StatA = 50;
        startingStats.StatB = 50;
        startingStats.StatC = 50;
        AdjustStats(startingStats);

        sinkA = -10;
        sinkB = -10;
        sinkC = -10;
    }

}
