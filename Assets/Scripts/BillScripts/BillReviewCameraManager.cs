using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillReviewCameraManager : MonoBehaviour
{

    public static BillReviewCameraManager Instance;
    public float camSpeed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(1, 0, 0) * (Time.deltaTime * camSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(1, 0, 0) * (Time.deltaTime * camSpeed);
        }
    }
}
