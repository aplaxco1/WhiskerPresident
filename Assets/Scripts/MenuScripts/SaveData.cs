using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

// NOTE: Used parts of my other Unity project's save/load code here -Justin

[Serializable]
public class SaveData
{
    private static SaveData instance = new SaveData();
    private static Settings settingsInst = new Settings();
    
    public StatVector statVector = new StatVector();
    public int dayProgression = 0;

    private SaveData()
    {
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
            Debug.LogError("SAVEDATA - SAVESETTINGS: Cannot access VolumeSlider instance.");
        }
        
        if (SettingsResolution.Instance != null)
        {
            resToSave = SettingsResolution.Instance.currentResolution;
        }
        else
        {
            Debug.LogError("SAVEDATA - SAVESETTINGS: Cannot access SettingsResolution instance.");
        }
        
        settingsInst = new Settings
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
        jsonSerializer.WriteObject(jsonStream, settingsInst);
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
        try
        {
            fileStream = File.OpenRead(Application.persistentDataPath + "/settings" + ".set");
        }
        catch (IOException ioe)
        {
            Debug.LogError(
                "SAVEDATA - LOADSETTINGS: IO error while loading file, loading default settings."
                + ": "
                + ioe.Message
            );
            LoadDefaultSettings();
            return;
        }

        try
        {
            settingsInst = (Settings) jsonSerializer.ReadObject(fileStream);
        }
        catch (SerializationException)
        {
            Debug.LogError("SAVEDATA - LOADSETTINGS: Something is wrong with the settings file, ignoring it.");
            LoadDefaultSettings();
            return;
        }

        if (VolumeSlider.Instance != null)
        {
            VolumeSlider.Instance.currentVolume = settingsInst.volume;
            VolumeSlider.Instance.changeVolume(settingsInst.volume);
        }
        else
        {
            Debug.LogError("SAVEDATA - LOADSETTINGS: Cannot access VolumeSlider instance.");
        }
        
        if (SettingsResolution.Instance != null)
        {
            SettingsResolution.Instance.SetRes(settingsInst.resolution);
        }
        else
        {
            Debug.LogError("SAVEDATA - LOADSETTINGS: Cannot access SettingsResolution instance.");
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
            Debug.LogError("SAVEDATA - SAVETOFILE: Cannot access StatManager instance.");
        }

        instance = new SaveData
        {
            dayProgression = EnvironmentManager.Instance.day,
            statVector = statVectorToSave,
        };
        
        // write to file
        var jsonSerializer = new DataContractJsonSerializer(
            typeof(SaveData)
        );
        var jsonStream = new MemoryStream();
        jsonSerializer.WriteObject(jsonStream, instance);
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
            typeof(SaveData)
        );
        FileStream fileStream;
        try
        {
            fileStream = File.OpenRead(Application.persistentDataPath + "/save" + saveNum + ".sav");
        }
        catch (IOException ioe)
        {
            Debug.LogError(
                "SAVEDATA - LOADFROMFILE: IO error while loading save file, using default save data."
                + saveNum
                + ": "
                + ioe.Message
            );
            LoadDefaultSave();
            return;
        }

        try
        {
            instance = (SaveData) jsonSerializer.ReadObject(fileStream);
        }
        catch (SerializationException)
        {
            Debug.LogError(
                "SAVEDATA - LOADFROMFILE: Something is wrong with the read save file, using default data."
            );
            LoadDefaultSave();
            return;
        }

        if (StatManager.Instance != null)
        {
            StatManager.Instance.SetStats(instance.statVector);
        }
        else
        {
            Debug.LogError(
                "SAVEDATA - LOADFROMFILE: Cannot access StatManager instance."
            );
        }
        
        if (EnvironmentManager.Instance != null)
        {
            EnvironmentManager.Instance.day = instance.dayProgression;
        }
        else
        {
            Debug.LogError(
                "SAVEDATA - LOADFROMFILE: Cannot access EnvironmentManager instance."
            );
        }
    }

    private static void LoadDefaultSave()
    {
        instance = new SaveData();
        instance.dayProgression = 0;
        instance.statVector = new StatVector();
        
        if (StatManager.Instance != null)
        {
            StatManager.Instance.SetStats(instance.statVector);
        }
    }
    
    private static void LoadDefaultSettings()
    {
        settingsInst = new Settings();
        settingsInst.volume = 0.5f;
        settingsInst.resolution = SettingsData.Resolution1;
    }

    public class Settings
    {
        public float volume;
        public SettingsData.Resolution resolution;
    }
}