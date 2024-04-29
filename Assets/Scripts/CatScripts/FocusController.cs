using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Random = UnityEngine.Random;

//using System.Diagnostics;
using UnityEngine;

public class FocusController : MonoBehaviour
{
    //private Coroutine moveCoroutine;

    // reference to laser pointer script
    public LaserPointer laserPointer;

    // Reference to the GameObject to move
    public GameObject objectToMove;

    // Speed of the movement
    //public float moveSpeed = 5f;

    public float interval = 5f;       // Desired interval between calls
    public float varianceRange = 2f;  // Variance range

    private float timer = 0f;

    //private float hackSolution = 0.8f;

    //public Vector3 ballOffset = new Vector3(2f, 2f, 2f);

    public Vector3 lastPoint;
    public Vector3 randomPoint;
    public Vector3 minRange = new Vector3(-0.80f, 0.80f, -0.50f);
    public Vector3 maxRange = new Vector3(0.80f, 0.80f, 0.80f);


    // for keeping track of laser pointer movement
    private Vector3 previousPointerLocation = new Vector3(0, 0, 0);
    private bool wasOnTable = false;
    public float pointerSpeed = 0;
    public float focusLevel = 0.1f;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Call your helper function or perform any desired task
            randomPoint = GenerateRandomVector();
            MoveObjectTo(randomPoint);

            // Reset the timer
            timer = interval + Random.Range(-varianceRange, varianceRange);
        }

        if (TelephoneDistraction.isActive) {
            MoveObjectTo(TelephoneDistraction.distractionPosition);
        }

        if (ClockDistraction.isActive) {
            MoveObjectTo(ClockDistraction.distractionPosition);
        }

        // Check if the player has clicked the mouse
        if (laserPointer.isOnDesk && !TelephoneDistraction.isActive && !ClockDistraction.isActive)
        {
            // Cast a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits any colliders
            if (Physics.Raycast(ray, out hit))
            {
                // Debug.Log("hit");
                lastPoint = hit.point;
                // Move the referenced object towards the click position
                MoveObjectTo(laserPointer.laserDeskLocation);

                pointerSpeed = wasOnTable ? Mathf.Clamp((laserPointer.laserDeskLocation - previousPointerLocation).magnitude/Time.deltaTime, 0.0f, 16.0f) : 1;
                wasOnTable = true;
                
                previousPointerLocation = laserPointer.laserDeskLocation;
            }
            else { wasOnTable = false; pointerSpeed = 0; }
        }
        else { wasOnTable = false; pointerSpeed = 0; }
        focusLevel = Mathf.Clamp(Mathf.Lerp(focusLevel, Mathf.Clamp(pointerSpeed/16.0f * 5f, 1f, 20f),Time.deltaTime*1.2f), 1f, 5f);
    }

    void MoveObjectTo(Vector3 targetPosition)
    {
        // Check if the referenced object is assigned
        if (objectToMove != null)
        {
            objectToMove.transform.position = targetPosition;
        }
        /*else
        {
            Debug.LogWarning("Object to move not assigned. Please assign an object in the Inspector.");
        }*/
    }

    Vector3 GenerateRandomVector()
    {
        // Generate random values within the specified ranges for x, y, and z
        float x = (float)Random.Range(minRange.x, maxRange.x);
        float y = (float)Random.Range(minRange.y, maxRange.y);
        float z = (float)Random.Range(minRange.z, maxRange.z);

        return new Vector3(x, y, z);
    }
}