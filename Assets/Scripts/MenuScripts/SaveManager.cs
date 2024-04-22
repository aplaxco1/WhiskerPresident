using System;
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

    [Serializable]
    public class SaveData
    {
        [DataMember]
        public StatVector statVector = new StatVector();
        public int dayProgression = 0;
    }
    
    [Serializable]
    public class Settings
    {
        public float volume;
        
        [DataMember]
        public SettingsData.Resolution resolution;
    }

    
    //Temp
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
            SaveToFile(1);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadFromFile(1);
        }
    }

    public static void SaveSettings()
    {
        float volumeToSave = 0.5f;
        SettingsData.Resolution resToSave = SettingsData.Resolution1;

        if (VolumeSlider.Instance != null)
        {
            volumeToSave = VolumeSlider.Instance.currentVolume;
        }
        else
        {
            Debug.LogError("SAVEMANAGER - SAVESETTINGS: Cannot access VolumeSlider instance.");
        }
        
        if (SettingsResolution.Instance != null)
        {
            resToSave = SettingsResolution.Instance.currentResolution;
        }
        else
        {
            Debug.LogError("SAVEMANAGER - SAVESETTINGS: Cannot access SettingsResolution instance.");
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

    public static void LoadSettings()
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
        catch (SerializationException)
        {
            Debug.LogError("SAVEMANAGER - LOADSETTINGS: Something is wrong with the settings file, ignoring it.");
            LoadDefaultSettings();
            return;
        }

        if (VolumeSlider.Instance != null)
        {
            VolumeSlider.Instance.currentVolume = settingsInstance.volume;
            VolumeSlider.Instance.changeVolume(settingsInstance.volume);
            VolumeSlider.Instance.updateVolume();
            VolumeSlider.Instance.volumeSlider.value = settingsInstance.volume;
        }
        else
        {
            Debug.LogError("SAVEMANAGER - LOADSETTINGS: Cannot access VolumeSlider instance.");
        }
        
        if (SettingsResolution.Instance != null)
        {
            SettingsResolution.Instance.SetRes(settingsInstance.resolution);
        }
        else
        {
            Debug.LogError("SAVEMANAGER - LOADSETTINGS: Cannot access SettingsResolution instance.");
        }
    }

    public static void SaveToFile(int saveNum)
    {
        StatVector statVectorToSave = new StatVector();
        if (StatManager.Instance != null)
        {
            statVectorToSave = StatManager.Instance.GetStats();
        }
        else
        {
            Debug.LogError("SAVEMANAGER - SAVETOFILE: Cannot access StatManager instance.");
        }

        saveInstance = new SaveData
        {
            dayProgression = EnvironmentManager.Instance.day,
            statVector = statVectorToSave,
        };
        
        // write to file
        var jsonSerializer = new DataContractJsonSerializer(
            typeof(SaveManager)
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

    public static void LoadFromFile(int saveNum)
    {
        var jsonSerializer = new DataContractJsonSerializer(
            typeof(SaveManager)
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
        catch (SerializationException)
        {
            Debug.LogError(
                "SAVEMANAGER - LOADFROMFILE: Something is wrong with the read save file, using default data."
            );
            LoadDefaultSave();
            return;
        }

        if (StatManager.Instance != null)
        {
            StatManager.Instance.SetStats(saveInstance.statVector);
        }
        else
        {
            Debug.LogError(
                "SAVEMANAGER - LOADFROMFILE: Cannot access StatManager instance."
            );
        }
        
        if (EnvironmentManager.Instance != null)
        {
            EnvironmentManager.Instance.day = saveInstance.dayProgression;
        }
        else
        {
            Debug.LogError(
                "SAVEMANAGER - LOADFROMFILE: Cannot access EnvironmentManager instance."
            );
        }
    }

    private static void LoadDefaultSave()
    {
        saveInstance = new SaveData();
        saveInstance.dayProgression = 0;
        saveInstance.statVector = new StatVector();
        
        if (StatManager.Instance != null)
        {
            StatManager.Instance.SetStats(saveInstance.statVector);
        }
        else
        {
            Debug.LogError(
                "SAVEMANAGER - LOADDEFAULTSAVE: Cannot access StatManager instance."
            );
        }
        
        if (EnvironmentManager.Instance != null)
        {
            EnvironmentManager.Instance.day = saveInstance.dayProgression;
        }
        else
        {
            Debug.LogError(
                "SAVEMANAGER - LOADDEFAULTSAVE: Cannot access EnvironmentManager instance."
            );
        }
    }
    
    private static void LoadDefaultSettings()
    {
        settingsInstance = new Settings();
        settingsInstance.volume = 0.5f;
        settingsInstance.resolution = SettingsData.Resolution1;
    }


}