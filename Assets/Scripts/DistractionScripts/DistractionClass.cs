using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractionClass : MonoBehaviour
{

    public DistractionManager distractionManager;

    [Header("Distraction Variables")]
    public bool isActive = false;
    public Vector3 distractionPosition;
    public float attentionLevel; // NOTE: If a frenzy distraction, it should have an attention level of 0
    public bool frenzyDistraction;

    // variables for handling time interval between next distraction trigger
    public float timer = 0;
    public float minTime;
    public float maxTime;
    public float nextEvent;

    public virtual void distractionEvent() { }
    public virtual bool checkStop() { return true; }
}
