using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDistraction : DistractionClass
{

    public float distractionDuration;
    public float distractionTimer;
    public GameObject mouseObj;
    public AudioSource squeakSource;
    
    // Start is called before the first frame update
    void Start()
    {
        minTime = 20;
        maxTime = 90;
        nextEvent = Random.Range(minTime, maxTime);
        distractionPosition = mouseObj.transform.position;
        attentionLevel = 0.98f;
        frenzyDistraction = false;
        distractionDuration = Random.Range(5, 10);
        distractionTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextEvent) {
            isActive = true;
        }

        if (isActive) {
            distractionEvent();
        }
        
    }

    public override void distractionEvent() {
        mouseObj.SetActive(true);

        if (checkStop()) {
            isActive = false;
            timer = 0;
            nextEvent = Random.Range(minTime, maxTime);
            mouseObj.SetActive(false);
            distractionDuration = Random.Range(5, 10);
            distractionTimer = 0;
        }
    }

    public override bool checkStop() {
        distractionTimer += Time.deltaTime;
        if (distractionTimer >= distractionDuration) {
            return true;
        }
        return false;
    }
}
