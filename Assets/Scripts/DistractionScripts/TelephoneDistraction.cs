using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TelephoneDistraction : DistractionClass
{

    public AudioSource ringSource;

    [Header("Shaking Animation")]
    // following is for the shaking
    private Vector3 startPosition;
    private Vector3 randomPosition;
    [Range(0f, 0.1f)]
    private float phone_vibrate_distance = 0.005f; // change this variable to increase amount of shaking!
    [Range(0f, 2f)]
    public float _time = 0.2f;
    [Range(0f, 0.1f)]
    public float _delayBetweenShakes = 0.1f;
    
    public PawCollision pawCollisionDetection;


    // Start is called before the first frame update
    void Start()
    {
        minTime = 10;
        maxTime = 25;
        if (SaveManager.Instance.currentSaveData.dayInfo.day == 2) {
            minTime = 8;
            maxTime = 15;
        }
        nextEvent = Random.Range(minTime, maxTime);
        distractionPosition = transform.position;
        startPosition = transform.position; // the og spot of the phone before it starts shaking.
        attentionLevel = 1f;
        frenzyDistraction = false; 
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextEvent && !isActive) {
            isActive = true;
            distractionManager.checkActiveDistractions();
        }

        if (isActive) {
            distractionEvent();
            
            StartCoroutine(Shake());
            
        }
    }
    private IEnumerator Shake()
    {
        //Debug.Log("TRYNA SHAKE: " + phone_vibrate_distance);
        while (isActive)
        {
            randomPosition = startPosition + (Random.insideUnitSphere * phone_vibrate_distance);

            transform.position = randomPosition;
            

            if (_delayBetweenShakes > 0f)
            {
                yield return new WaitForSeconds(_delayBetweenShakes);
            }
            else
            {
                yield return null;
            }
        }

        transform.position = startPosition;

    }

    public override void distractionEvent() {
        if (!ringSource.isPlaying) {
            ringSource.Play();
        }
        ringSource.loop = true;

        if (checkStop()) {
            ringSource.Stop();
            transform.position = startPosition; // set the phone back to the position it was before it started shaking
            timer = 0;
            nextEvent = Random.Range(minTime, maxTime);
        }
    }

    // temporary way to stop distraction (right click phone)
    public override bool checkStop() {
        if (pawCollisionDetection && pawCollisionDetection.surface.CompareTag("Phone")) {
            isActive = false;
            distractionManager.checkActiveDistractions();
            return true;
        }
        return false;
        /*
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
        */
    }

}
