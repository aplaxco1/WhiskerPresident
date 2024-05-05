using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelephoneDistraction : DistractionClass
{

    public AudioSource ringSource;

    // Start is called before the first frame update
    void Start()
    {
        minTime = 30;
        maxTime = 50;
        nextEvent = Random.Range(minTime, maxTime);
        distractionPosition = transform.position;
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
        if (!ringSource.isPlaying) {
            ringSource.Play();
        }
        ringSource.loop = true;

        if (checkStop()) {
            ringSource.Stop();
            isActive = false;
            timer = 0;
            nextEvent = Random.Range(minTime, maxTime);
        }
    }

    // temporary way to stop distraction (right click phone)
    public override bool checkStop() {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject.CompareTag("Phone")) {
                    return true;
                }
            }
        }
        return false;
    }

}
