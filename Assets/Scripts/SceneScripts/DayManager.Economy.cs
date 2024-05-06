using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DayManager
{
    public int sinkA;
    public int sinkB;
    public int sinkC;
    public void DayEnd()
    {
        statA += sinkA;
        statB += sinkB;
        statC += sinkC;
        
        if (LowStatCheck())
        {
            StatLow();
        }
        else
        {
            impeached = false;
        }

    }

    bool LowStatCheck()
    {
        return (statA <= 0 || statB <= 0 || statC <= 0);
    }

    public void DayStart()
    {
        
    }
    
    public void StatLow()
    {
        if (impeached)
        {
            TriggerLoss();
        }
        else
        {
            impeached = true;
        }
    }

    public void TriggerLoss()
    {
        print("YOU LOSE");
    }
}
