using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{

    public static EnvironmentManager Instance;

    public int day;

    private void Awake()
    {
        Instance = this;
    }
}
