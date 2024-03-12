using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillReviewCameraManager : MonoBehaviour
{

    public static BillReviewCameraManager Instance;
    public float camSpeed;

    private float xMin;
    private float xMax;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        xMin = 0.5f;
    }

    public void SetLastBill(float lastX)
    {
        xMax = lastX - 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) && transform.position.x > xMin)
        {
            transform.position -= new Vector3(1, 0, 0) * (Time.deltaTime * camSpeed);
        }
        if (Input.GetKey(KeyCode.D) && transform.position.x < xMax)
        {
            transform.position += new Vector3(1, 0, 0) * (Time.deltaTime * camSpeed);
        }
    }
}
