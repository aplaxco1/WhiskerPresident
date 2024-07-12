using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

public class CatAimScript : MonoBehaviour
{
    public Transform catAim; // The object to be transformed
    public GameObject catFocus; // The sphere object defining the area

    private float sphereRadius;

    private void Start()
    {
        // Get the radius of the sphere
        if (catFocus != null)
        {
            SphereCollider sphereCollider = catFocus.GetComponent<SphereCollider>();
            if (sphereCollider != null)
            {
                sphereRadius = sphereCollider.radius;
            }
            else
            {
                Debug.LogError("Sphere object does not have a SphereCollider component.");
            }
        }
        else
        {
            Debug.LogError("Sphere object is not assigned.");
        }
    }

    private void Update()
    {
        if (catAim != null && catFocus != null)
        {
            // Generate random position inside the sphere
            Vector3 randomPosition = Random.insideUnitSphere * sphereRadius;

            // Set object position
            catAim.position = randomPosition;
        }
    }

}
