using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockDistraction : DistractionClass
{

    public AudioSource tickSource;
    public Animator birdAnim;
    public LaserPointer laser;

    // Start is called before the first frame update
    void Start()
    {
        minTime = 50;
        maxTime = 60;
        nextEvent = Random.Range(minTime, maxTime);
        distractionPosition = transform.position;
        attentionLevel = 0f;
        frenzyDistraction = true;
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
        if (!tickSource.isPlaying) {
            tickSource.Play();
        }
        tickSource.loop = true;

        birdAnim.Play("Cuckoo");
        //springAnim.Play("Spring");

        if (checkStop()) {
            AudioManager.Instance.Play(SoundName.button, 0.5f);
            tickSource.Stop();
            birdAnim.Play("Stop");
            //springAnim.Play("Stop");
            isActive = false;
            timer = 0;
            nextEvent = Random.Range(minTime, maxTime);
        }
    }

    // stop distraction by right clicking clock
    public override bool checkStop() {
        if (laser.isActive) {
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
