using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DayManager
{

        
    public void DayEnd()
    {
        dayInfo.statA += dayInfo.sinkA;
        dayInfo.statB += dayInfo.sinkB;
        dayInfo.statC += dayInfo.sinkC;
        
        if (LowStatCheck())
        {
            StatLow();
        }
        else
        {
            dayInfo.impeached = false;
        }

    }

    bool LowStatCheck()
    {
        return (dayInfo.statA <= 0 || dayInfo.statB <= 0 || dayInfo.statC <= 0);
    }

    public void DayStart()
    {
        
    }
    
    public void StatLow()
    {
        if (dayInfo.impeached)
        {
            TriggerLoss();
        }
        else
        {
            dayInfo.impeached = true;
        }
    }

    public void TriggerLoss()
    {
        print("YOU LOSE");
    }
}
