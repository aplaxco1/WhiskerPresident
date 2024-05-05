using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractionClass : MonoBehaviour
{
    public bool isActive = false;
    public Vector3 distractionPosition;

    // variables for handling time interval between next distraction trigger
    public float timer = 0;
    public float minTime;
    public float maxTime;
    public float nextEvent;

    public virtual void distractionEvent() { }
    public virtual bool checkStop() { return true; }
}
