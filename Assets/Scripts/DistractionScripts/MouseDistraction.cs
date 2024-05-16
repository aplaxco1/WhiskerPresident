using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDistraction : DistractionClass
{

    public float distractionDuration;
    public float distractionTimer;
    public GameObject mouseObj;
    public AudioSource squeakSource;

    [Header("Mouse Movement")]
    public Vector3 startPos;
    public Vector3 endPos;
    private Vector3 currTarget;
    public float moveSpeed = 1.3f;
    public float rotateSpeed = 800f;
    private Vector3 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        minTime = 20;
        maxTime = 90;
        nextEvent = Random.Range(minTime, maxTime);
        distractionPosition = mouseObj.transform.position;
        attentionLevel = 0.98f;
        frenzyDistraction = false;
        distractionDuration = Random.Range(8, 16);
        distractionTimer = 0;
        // movement stuff
        startPos = mouseObj.transform.position;
        endPos = new Vector3(mouseObj.transform.position.x - 1.3f, mouseObj.transform.position.y, mouseObj.transform.position.z);
        currTarget = endPos;
        direction = new Vector3(270,180,0);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextEvent && !isActive) {
            isActive = true;
            mouseObj.transform.position = startPos;
        }

        if (isActive) {
            distractionEvent();
        }
        
    }

    public override void distractionEvent() {
        mouseObj.SetActive(true);

        // makes mouse swap directions
        if (mouseObj.transform.position == startPos) {
            currTarget = endPos;
            direction = new Vector3(270,180,0);
        }
        if (mouseObj.transform.position == endPos) {
            currTarget = startPos;
            direction = new Vector3(270,0,0);
        }

        // rotate
        mouseObj.transform.rotation = Quaternion.RotateTowards(mouseObj.transform.rotation, Quaternion.Euler(direction), rotateSpeed * Time.deltaTime);

        // move mouse towards current target position
        mouseObj.transform.position = Vector3.MoveTowards(mouseObj.transform.position, currTarget, Time.deltaTime * moveSpeed);
        distractionPosition = mouseObj.transform.position;

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
