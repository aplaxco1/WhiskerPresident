using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelephoneDistraction : MonoBehaviour
{

    static public bool isActive = false;

    private float timer = 0;
    private float nextRing;
    public AudioSource ringSource;

    // Start is called before the first frame update
    void Start()
    {
        nextRing = Random.Range(10, 20);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextRing) {
            isActive = true;
        }

        if (isActive) {
            phoneRing();
        }
        else {
        }

    }

    void phoneRing() {
        if (!ringSource.isPlaying) {
            ringSource.Play();
        }
        ringSource.loop = true;

        if (checkStop()) {
            ringSource.Stop();
            isActive = false;
            timer = 0;
            nextRing = Random.Range(10, 20);
        }
    }

    // temporary way to stop distraction (right click phone)
    bool checkStop() {
        if (Input.GetMouseButtonDown(1)) {
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
