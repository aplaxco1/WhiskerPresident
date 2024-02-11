using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour
{

    private bool isOn = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // check right click
        if (Input.GetMouseButtonDown(1)) {
            isOn = !isOn;
        }

        // move laser if on
        if (isOn) {
            // create ray from camera to mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float distance = 100f; // temp
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                Debug.Log(hit.point);
            }
        }
    }
}
