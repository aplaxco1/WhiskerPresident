using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockDistraction : MonoBehaviour
{

    static public bool isActive = false;
    static public Vector3 distractionPosition;

    private float timer = 0;
    private float nextTick;
    public AudioSource tickSource;

    // Start is called before the first frame update
    void Start()
    {
        nextTick = Random.Range(20, 30);
        distractionPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextTick) {
            isActive = true;
        }

        if (isActive) {
            clockTick();
        }
        else {
        }

    }

    void clockTick() {
        if (!tickSource.isPlaying) {
            tickSource.Play();
        }
        tickSource.loop = true;

        if (checkStop()) {
            tickSource.Stop();
            isActive = false;
            timer = 0;
            nextTick = Random.Range(20, 30);
        }
    }

    // stop distraction by right clicking clock
    bool checkStop() {
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject.CompareTag("Clock")) {
                    return true;
                }
            }
        }
        return false;
    }

}
