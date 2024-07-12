using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDistraction : DistractionClass
{

    public float distractionDuration;
    public float distractionTimer;
    public GameObject mouseObj;
    public GameObject mouse;

    [Header("Mouse Movement")]
    public List<Vector3> positions;
    private int currTarget;
    public float moveSpeed = 1.3f;
    public float rotateSpeed = 8f;
    private Quaternion rotation;
    private Vector3 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        minTime = 20;
        maxTime = 40;
        if (SaveManager.Instance.currentSaveData.dayInfo.day >= 4) {
            minTime = 10;
            maxTime = 30;
        }
        nextEvent = Random.Range(minTime, maxTime);
        distractionPosition = mouseObj.transform.position;
        attentionLevel = 0.75f;
        frenzyDistraction = false;
        distractionDuration = Random.Range(10, 20);
        distractionTimer = 0;
        // movement stuff
        positions.Add(mouseObj.transform.position);
        positions.Add(new Vector3(mouseObj.transform.position.x - 1.3f, mouseObj.transform.position.y, mouseObj.transform.position.z));
        positions.Add(new Vector3(mouseObj.transform.position.x - 0.65f, mouseObj.transform.position.y, mouseObj.transform.position.z + 0.5f));
        currTarget = Random.Range(0, 3);
        direction = (mouseObj.transform.position - positions[currTarget]).normalized;
        rotation = Quaternion.LookRotation(direction);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextEvent && !isActive) {
            isActive = true;
            direction = (mouseObj.transform.position - positions[currTarget]).normalized;
            rotation = Quaternion.LookRotation(direction);
            distractionManager.checkActiveDistractions();
        }

        if (isActive) {
            distractionEvent();
        }
        
    }

    public override void distractionEvent() {
        mouse.SetActive(true);

        // makes mouse swap directions
        if (mouseObj.transform.position == positions[currTarget]) {
            currTarget = Random.Range(0, 3);
            // update look vector
            direction = (mouseObj.transform.position - positions[currTarget]).normalized;
            rotation = Quaternion.LookRotation(direction);
        }

        // rotate mouse towards target position
        mouseObj.transform.localRotation = Quaternion.Slerp(mouseObj.transform.rotation, rotation, rotateSpeed*Time.deltaTime);

        // move mouse towards current target position
        mouseObj.transform.position = Vector3.MoveTowards(mouseObj.transform.position, positions[currTarget], Time.deltaTime * moveSpeed);

        // update distraction position for cat focus
        distractionPosition = mouseObj.transform.position;

        if (checkStop()) {
            timer = 0;
            nextEvent = Random.Range(minTime, maxTime);
            mouse.SetActive(false);
            distractionDuration = Random.Range(5, 10);
            distractionTimer = 0;
        }
    }

    public override bool checkStop() {
        distractionTimer += Time.deltaTime;
        if (distractionTimer >= distractionDuration) {
            isActive = false;
            distractionManager.checkActiveDistractions();
            currTarget = Random.Range(0, 3);
            mouseObj.transform.position = positions[Random.Range(0, 3)];
            return true;
        }
        return false;
    }
}
