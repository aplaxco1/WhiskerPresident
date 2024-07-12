using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class DayManager
{

        
    public void DayEnd()
    {
        dayInfo.statA += dayInfo.sinkA;
        dayInfo.statB += dayInfo.sinkB;
        dayInfo.statC += dayInfo.sinkC;

        // check lose
        if (CheckLowStats())
        {
            TriggerLoss();
        }

        // check end of game
        if (dayInfo.day >= 5) {
            TriggerEnd();
        }

        if (dayInfo.day < 5 && !CheckLowStats()) {
            // else just continue normally
            dayInfo.day++;
            
            SceneTransitionManager.TransitionNextScene();
        }
    }

    public bool CheckLowStats()
    {
        return dayInfo.statA <= 0 || dayInfo.statB <= 0 || dayInfo.statC <= 0;
    }


    public void DayStart()
    {
        
    }
    
    // public void StatLow()
    // {
    //     if (dayInfo.impeached)
    //     {
    //         TriggerLoss();
    //     }
    //     else
    //     {
    //         dayInfo.impeached = true;
    //     }
    // }

    public void TriggerLoss()
    {
        EndOfGame.looseGame = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void TriggerEnd() 
    {
        EndOfGame.winGame = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void AdjustSinks(SinkVector sinkVector)
    {
        AddToSinkA(sinkVector.SinkA);
        AddToSinkB(sinkVector.SinkB);
        AddToSinkC(sinkVector.SinkC);
    }

    private void AddToSinkA(int factor)
    {
        dayInfo.sinkA += factor;
        dayInfo.sinkA = Mathf.Clamp(dayInfo.sinkA, -100, 100);
    }
    
    private void AddToSinkB(int factor)
    {
        dayInfo.sinkB += factor;
        dayInfo.sinkB = Mathf.Clamp(dayInfo.sinkB, -100, 100);
    }
    
    private void AddToSinkC(int factor)
    {
        dayInfo.sinkC += factor;
        dayInfo.sinkC = Mathf.Clamp(dayInfo.sinkC, -100, 100);
    }

    public int GetSinkA()
    {
        return dayInfo.sinkA;
    }
    
    public int GetSinkB()
    {
        return dayInfo.sinkB;
    }
    
    public int GetSinkC()
    {
        return dayInfo.sinkC;
    }

    public SinkVector GetSinks()
    {
        SinkVector ret = new SinkVector
        {
            SinkA = GetSinkA(),
            SinkB = GetSinkB(),
            SinkC = GetSinkC()
        };
        return ret;
    }
    
    public void SetSinks(SinkVector sinkVector)
    {
        dayInfo.sinkA = sinkVector.SinkA;
        dayInfo.sinkB = sinkVector.SinkB;
        dayInfo.sinkC = sinkVector.SinkC;
    }

}
