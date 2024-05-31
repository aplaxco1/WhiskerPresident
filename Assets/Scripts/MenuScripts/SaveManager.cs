using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

// NOTE: Used parts of my other Unity project's save/load code here -Justin

public partial class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public SaveData currentSaveData;
    public SettingsData currentSettingsData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        LoadFromFile();
        LoadSettings();
    }

    //Temporary save/load controls
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SaveSettings();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            LoadSettings();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveToFile();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadFromFile();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            ResetSettings();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            ResetSaveData();
        }
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            DayManager.Instance.InitializeDefaultStats();
        }
    }
}