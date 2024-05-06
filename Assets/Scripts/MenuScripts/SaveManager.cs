﻿using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;
using UnityEngine.PlayerLoop;

// NOTE: Used parts of my other Unity project's save/load code here -Justin

public class SaveManager : MonoBehaviour
{
    private static SaveData saveInstance = new SaveData();
    private static Settings settingsInstance = new Settings();
    public static SaveManager Instance;

    public SaveData currentSaveData;
    public Settings currentSettings;

    [Serializable]
    public class SaveData
    {
        [DataMember]
        public StatVector statVector = new StatVector();
        
        [DataMember]
        public int dayProgression = 0;
    }
    
    [Serializable]
    public class Settings
    {
        [DataMember]
        public float volume;
        
        [DataMember]
        public SettingsData.Resolution resolution;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        LoadSettings();
    }

    public void SaveSettings()
    {
        float volumeToSave = 0.5f;
        SettingsData.Resolution resToSave = SettingsData.Resolution1;

        if (VolumeSlider.Instance != null)
        {
            volumeToSave = VolumeSlider.Instance.currentVolume;
        }
        else
        {
            Debug.LogWarning("SAVEMANAGER - SAVESETTINGS: Cannot access VolumeSlider instance.");
        }
        
        if (SettingsResolution.Instance != null)
        {
            resToSave = SettingsResolution.Instance.currentResolution;
        }
        else
        {
            Debug.LogWarning("SAVEMANAGER - SAVESETTINGS: Cannot access SettingsResolution instance.");
        }
        
        settingsInstance = new Settings
        {
            // update stuff here, then save to object file
            volume = volumeToSave,
            resolution = resToSave,
        };

        // write to file
        var jsonSerializer = new DataContractJsonSerializer(
            typeof(Settings)
        );
        var jsonStream = new MemoryStream();
        jsonSerializer.WriteObject(jsonStream, settingsInstance);
        Debug.Log("Attempting to save settings file to " + Application.persistentDataPath + "/settings" + ".set");
        var fileStream = File.Create(Application.persistentDataPath + "/settings" + ".set");
        jsonStream.Seek(0, SeekOrigin.Begin);
        jsonStream.CopyTo(fileStream);
        fileStream.Close();
    }

    public void LoadSettings()
    {
        var jsonSerializer = new DataContractJsonSerializer(
            typeof(Settings)
        );
        FileStream fileStream;
        Debug.Log("Attempting to load settings file from " + Application.persistentDataPath + "/settings" + ".set");
        try
        {
            fileStream = File.OpenRead(Application.persistentDataPath + "/settings" + ".set");
        }
        catch (IOException ioe)
        {
            Debug.LogError(
                "SAVEMANAGER - LOADSETTINGS: IO error while loading file, loading default settings."
                + ": "
                + ioe.Message
            );
            LoadDefaultSettings();
            return;
        }

        try
        {
            settingsInstance = (Settings) jsonSerializer.ReadObject(fileStream);
        }
        catch (SerializationException e)
        {
            Debug.LogError(e);
            Debug.LogError("SAVEMANAGER - LOADSETTINGS: Something is wrong with the settings file, ignoring it.");
            LoadDefaultSettings();
            return;
        }

        currentSettings = settingsInstance;

        if (VolumeSlider.Instance != null)
        {
            VolumeSlider.Instance.currentVolume = settingsInstance.volume;
            VolumeSlider.Instance.changeVolume(settingsInstance.volume);
            VolumeSlider.Instance.updateVolume();
            VolumeSlider.Instance.volumeSlider.value = settingsInstance.volume;
        }
        else
        {
            Debug.LogWarning("SAVEMANAGER - LOADSETTINGS: Cannot access VolumeSlider instance. (This is probably fine)");
        }
        
        if (SettingsResolution.Instance != null)
        {
            SettingsResolution.Instance.SetRes(settingsInstance.resolution);
        }
        else
        {
            Debug.LogWarning("SAVEMANAGER - LOADSETTINGS: Cannot access SettingsResolution instance.");
        }
    }

    public void SaveToFile(int saveNum = 1)
    {
        StatVector statVectorToSave = new StatVector();
        if (DayManager.Instance != null)
        {
            statVectorToSave = DayManager.Instance.GetStats();
        }
        else
        {
            Debug.LogWarning("SAVEMANAGER - SAVETOFILE: Cannot access DayManager instance.");
        }

        saveInstance = new SaveData
        {
            dayProgression = DayManager.Instance.day,
            statVector = statVectorToSave,
        };
        
        // write to file
        var jsonSerializer = new DataContractJsonSerializer(
            typeof(SaveData)
        );
        var jsonStream = new MemoryStream();
        jsonSerializer.WriteObject(jsonStream, saveInstance);
        Debug.Log("Attempting to save data file to " + Application.persistentDataPath + "/save" + saveNum + ".sav");

        var fileStream = File.Create(
            Application.persistentDataPath + "/save" + saveNum + ".sav"
        );
        jsonStream.Seek(0, SeekOrigin.Begin);
        jsonStream.CopyTo(fileStream);
        fileStream.Close();
    }

    public void LoadFromFile(int saveNum = 1)
    {
        var jsonSerializer = new DataContractJsonSerializer(
            typeof(SaveData)
        );
        FileStream fileStream;
        Debug.Log("Attempting to load data file from " + Application.persistentDataPath + "/save" + saveNum + ".sav");
        try
        {
            fileStream = File.OpenRead(Application.persistentDataPath + "/save" + saveNum + ".sav");
        }
        catch (IOException ioe)
        {
            Debug.LogError(
                "SAVEMANAGER - LOADFROMFILE: IO error while loading save file, using default save data."
                + saveNum
                + ": "
                + ioe.Message
            );
            LoadDefaultSave();
            return;
        }

        try
        {
            saveInstance = (SaveData) jsonSerializer.ReadObject(fileStream);
        }
        catch (SerializationException e)
        {
            Debug.LogError(e);
            Debug.LogError(
                "SAVEMANAGER - LOADFROMFILE: Something is wrong with the read save file, using default data."
            );
            LoadDefaultSave();
            return;
        }

        currentSaveData = saveInstance;

        if (DayManager.Instance != null)
        {
            DayManager.Instance.SetStats(saveInstance.statVector);
        }
        else
        {
            Debug.LogWarning(
                "SAVEMANAGER - LOADFROMFILE: Cannot access DayManager instance."
            );
        }
        
        if (DayManager.Instance != null)
        {
            DayManager.Instance.day = saveInstance.dayProgression;
        }
        else
        {
            Debug.LogWarning(
                "SAVEMANAGER - LOADFROMFILE: Cannot access DayManager instance."
            );
        }
    }

    private void LoadDefaultSave()
    {
        saveInstance = new SaveData();
        saveInstance.dayProgression = 0;
        saveInstance.statVector = new StatVector();
        
        if (DayManager.Instance != null)
        {
            DayManager.Instance.SetStats(saveInstance.statVector);
        }
        else
        {
            Debug.LogWarning(
                "SAVEMANAGER - LOADDEFAULTSAVE: Cannot access DayManager instance."
            );
        }
        
        if (DayManager.Instance != null)
        {
            DayManager.Instance.day = saveInstance.dayProgression;
        }
        else
        {
            Debug.LogWarning(
                "SAVEMANAGER - LOADDEFAULTSAVE: Cannot access DayManager instance."
            );
        }
    }
    
    private void LoadDefaultSettings()
    {
        settingsInstance = new Settings();
        settingsInstance.volume = 0.5f;
        settingsInstance.resolution = SettingsData.Resolution1;
    }


}