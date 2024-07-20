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
    public float lingerTimer = 1f;
    public float varianceRange = 2f;  // Variance range
    private float timer = 0f;

    //public Vector3 ballOffset = new Vector3(2f, 2f, 2f);

    public Vector3 lastPoint;
    public Vector3 randomPoint;
    public Vector3 minRange = new Vector3(-1.60f, 0.80f, -0.50f);
    public Vector3 maxRange = new Vector3(1.60f, 0.80f, 0.80f);


    [Header("Laser Pointer Reference")]
    // for keeping track of laser pointer movement
    public LaserPointer laserPointer;  // reference to laser pointer script
    public float focusLevel = 0.1f;

    [Header("Distraction Reference")]
    public DistractionManager distractions;


    [Header("attetion offsets")]
    public float offsetXRange = .2f; // Range for random offset in X direction
    //public float offsetYRange = 1f; // Range for random offset in Y direction
    public float offsetZRange = .2f; // Range for random offset in Z direction

    void Update()
    {
        timer -= Time.deltaTime;
        lingerTimer -= Time.deltaTime;

        // generate new random attention point
        if (timer <= 0f && lingerTimer <= 0f)
        {
            // Call your helper function or perform any desired task
            randomPoint = GenerateRandomVector();
            MoveObjectTo(randomPoint);

            // Reset the timer
            timer = interval + Random.Range(-varianceRange, varianceRange);
        }

        // 
        if (distractions.activeDistractions.Count > 0 && distractions.frenzyActive) {
            catGoCrazyMode();
        }

        // set focus level to object with highest attention level
        else if (distractions.activeDistractions.Count > 0) {
            // if no laser, or if distraction attention level is higher than laser, move attention point to distraction
            if ((laserPointer.isOnDesk && (distractions.activeDistractions[distractions.activeDistractions.Count - 1].attentionLevel > laserPointer.attentionLevel)) || !laserPointer.isOnDesk) {
                MoveObjectTo(distractions.activeDistractions[distractions.activeDistractions.Count - 1].distractionPosition);
            }
            // otherwise, if laser and laser attention level is higher than distraction, move attention point to laser
            else if (laserPointer.isOnDesk && (distractions.activeDistractions[distractions.activeDistractions.Count - 1].attentionLevel < laserPointer.attentionLevel)) {
                MoveObjectTo(laserPointer.laserDeskLocation);
            }
        }

        else if ((laserPointer.isOnDesk && laserPointer.attentionLevel > 0f)) {
            lingerTimer = 1f;
            MoveObjectTo(laserPointer.laserDeskLocation);
        }

        // cat is focused on laser pointer, no distractions
        // if ((laserPointer.isOnDesk && laserPointer.attentionLevel > 0f) && distractions.activeDistractions.Count == 0)
        // {
        //     MoveObjectTo(laserPointer.laserDeskLocation);
        // }

        // update focus level 
        focusLevel = Mathf.Clamp(Mathf.Lerp(focusLevel, Mathf.Clamp(laserPointer.pointerSpeed/16.0f * 5f, 1f, 20f),Time.deltaTime*1.2f), 1f, 5f);
    }

    void MoveObjectTo(Vector3 targetPosition)
    {
        Vector3 smackPos = targetPosition;
        // Check if the referenced object is assigned
        if (objectToMove != null)
        {
            // float randomOffsetX = Random.Range(-offsetXRange, offsetXRange);
            // float randomOffsetY = 0;
            // float randomOffsetZ = Random.Range(-offsetZRange, offsetZRange);
            // Vector3 offset = new Vector3(randomOffsetX, randomOffsetY, randomOffsetZ);

            //objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, smackPos + offset, interval);
            objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, smackPos, interval);
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

    void catGoCrazyMode() {
        focusLevel = 3f;
        interval = 1f;
        offsetXRange = 1f;
        offsetZRange = 1f;
    }

}