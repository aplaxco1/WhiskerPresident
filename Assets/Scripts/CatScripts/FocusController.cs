using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Random = UnityEngine.Random;

//using System.Diagnostics;
using UnityEngine;

public class FocusController : MonoBehaviour
{

    // Reference to the GameObject to move
    public GameObject objectToMove;

    [Header("Random Attention Point Generation")]
    public float interval = 5f;       // Desired interval between calls
    public float varianceRange = 2f;  // Variance range
    private float timer = 0f;

    //public Vector3 ballOffset = new Vector3(2f, 2f, 2f);

    public Vector3 lastPoint;
    public Vector3 randomPoint;
    public Vector3 minRange = new Vector3(-0.80f, 0.80f, -0.50f);
    public Vector3 maxRange = new Vector3(0.80f, 0.80f, 0.80f);


    [Header("Laser Pointer Reference")]
    // for keeping track of laser pointer movement
    public LaserPointer laserPointer;  // reference to laser pointer script
    public float focusLevel = 0.1f;

    void Update()
    {
        timer -= Time.deltaTime;

        // generate new random attention point
        if (timer <= 0f)
        {
            // Call your helper function or perform any desired task
            randomPoint = GenerateRandomVector();
            MoveObjectTo(randomPoint);

            // Reset the timer
            timer = interval + Random.Range(-varianceRange, varianceRange);
        }

        // cat is distracted -> will be changed when more distractions are added
        if (TelephoneDistraction.isActive) {
            MoveObjectTo(TelephoneDistraction.distractionPosition);
        }

        // cat is focused on laser pointer
        if (laserPointer.isOnDesk && !TelephoneDistraction.isActive)
        {
            MoveObjectTo(laserPointer.laserDeskLocation);
        }

        // update focus level 
        focusLevel = Mathf.Clamp(Mathf.Lerp(focusLevel, Mathf.Clamp(laserPointer.pointerSpeed/16.0f * 5f, 1f, 20f),Time.deltaTime*1.2f), 1f, 5f);
    }

    void MoveObjectTo(Vector3 targetPosition)
    {
        // Check if the referenced object is assigned
        if (objectToMove != null)
        {
            objectToMove.transform.position = targetPosition;
        }
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