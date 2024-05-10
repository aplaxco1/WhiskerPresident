using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayInfo {
    public int day;
    public bool impeached;

    public int sinkA;
    public int sinkB;
    public int sinkC;

    public int statA;
    public int statB;
    public int statC;
    
    public DayInfo() {

    }

    public string ConvertToString()
    {
        string ret = "";
        ret += "Day: " + day + "\n";
        ret += "Impeached: " + impeached + "\n";
        ret += "sinkA: " + sinkA + "\n";
        ret += "sinkB: " + sinkB + "\n";
        ret += "sinkC: " + sinkC + "\n";
        ret += "statA: " + statA + "\n";
        ret += "statB: " + statB + "\n";
        ret += "statC: " + statC + "\n";
        return ret;
    }
}
