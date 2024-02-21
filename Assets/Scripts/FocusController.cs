using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Random = UnityEngine.Random;

//using System.Diagnostics;
using UnityEngine;

public class FocusController : MonoBehaviour
{
    private Coroutine moveCoroutine;

    // reference to laser pointer script
    public LaserPointer laserPointer;

    // Reference to the GameObject to move
    public GameObject objectToMove;

    // Speed of the movement
    public float moveSpeed = 5f;

    public float interval = 5f;       // Desired interval between calls
    public float varianceRange = 2f;  // Variance range

    private float timer = 0f;

    private float hackSolution = 0.8f;

    //public Vector3 ballOffset = new Vector3(2f, 2f, 2f);

    public Vector3 lastPoint;
    public Vector3 randomPoint;
    public Vector3 minRange = new Vector3(-0.80f, 0.80f, -0.50f);
    public Vector3 maxRange = new Vector3(0.80f, 0.80f, 0.80f);


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

        // Check if the player has clicked the mouse
        if (laserPointer.isOnDesk)
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
            }
        }
    }

    void MoveObjectTo(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(objectToMove.transform.position, targetPosition);

        // Check if the referenced object is assigned
        if (objectToMove != null)
        {
            // Stop any existing movement coroutines on the referenced object
            //objectToMove.GetComponent<MoveToPosition>().StopMovement();
            //objectToMove.transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            //distance = Vector3.Distance(transform.position, targetPosition);

            objectToMove.transform.position = targetPosition;

            // Start a new coroutine to smoothly move the referenced object towards the target position
            //objectToMove.GetComponent<MoveToPosition>().MoveTo(targetPosition, moveSpeed);
        }
        else
        {
            //Debug.LogWarning("Object to move not assigned. Please assign an object in the Inspector.");
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