using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class ClockDistraction : DistractionClass
{

    public AudioSource tickSource;
    public Animator birdAnim;
    public Animator springAnim;

    // WII REMOTE STUFF AHHHH
    Wiimote mote;
    float mouseX;
    float mouseY;

    // Start is called before the first frame update
    void Start()
    {
        minTime = 5;
        maxTime = 6;
        nextEvent = Random.Range(minTime, maxTime);
        distractionPosition = transform.position;
        attentionLevel = 0f;
        frenzyDistraction = true;
        StartCoroutine(ActivateMote());
    }

    // Update is called once per frame
    void Update()
    {
        if (mote != null) {
            // float[] acell = mote.Accel.GetCalibratedAccelData();
            float[] pointer = mote.Ir.GetPointingPosition();

            mouseX = pointer[0] * 800f;
            mouseY = pointer[1] * 500f;
        }

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
        springAnim.Play("Spring");

        if (checkStop()) {
            AudioManager.Instance.Play(SoundName.button, 0.5f);
            tickSource.Stop();
            birdAnim.Play("Stop");
            springAnim.Play("Stop");
            isActive = false;
            timer = 0;
            nextEvent = Random.Range(minTime, maxTime);
        }
    }

    // stop distraction by right clicking clock
    public override bool checkStop() {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0) || mote.Button.b) {
            Ray ray;
            if (mote != null) { ray = Camera.main.ScreenPointToRay(new Vector2(mouseX, mouseY)); }
            else { ray = Camera.main.ScreenPointToRay(Input.mousePosition); }
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject.CompareTag("Clock")) {
                    return true;
                }
            }
        }
        return false;
    }

    IEnumerator ActivateMote() {
        yield return new WaitUntil(()=> WiimoteManager.HasWiimote());
        mote = WiimoteManager.Wiimotes[0];
    }

}
