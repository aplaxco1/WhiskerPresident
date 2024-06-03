using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

// NOTE: Used parts of my other Unity project's save/load code here -Justin

public partial class SaveManager
{
    [Serializable]
    public class SettingsData
    {
        [DataMember]
        public float masterVolume;
        public float musicVolume;
        public float soundsVolume;
        
        [DataMember]
        public global::SettingsData.Resolution resolution;
    }
    
    public void SaveSettings(SettingsData settingsDataOverride = null)
    {
        SettingsData settingsDataInstance = GetDefaultSettingsData();

        if (settingsDataOverride != null)
        {
            settingsDataInstance = settingsDataOverride;
        }
        else
        {
            if (VolumeManager.Instance != null)
            {
                settingsDataInstance.masterVolume = VolumeManager.Instance.currentMasterVolume;
                settingsDataInstance.musicVolume = VolumeManager.Instance.currentMusicVolume;
                settingsDataInstance.soundsVolume = VolumeManager.Instance.currentSoundsVolume;
            }
            else
            {
                Debug.LogWarning("SAVEMANAGER - SAVESETTINGS: Cannot access VolumeManager instance.");
            }
        
            if (SettingsResolution.Instance != null)
            {
                settingsDataInstance.resolution = SettingsResolution.Instance.currentResolution;
            }
            else
            {
                Debug.LogWarning("SAVEMANAGER - SAVESETTINGS: Cannot access SettingsResolution instance.");
            }
        }
        
        // write to file
        var jsonSerializer = new DataContractJsonSerializer(
            typeof(SettingsData)
        );
        var jsonStream = new MemoryStream();
        jsonSerializer.WriteObject(jsonStream, settingsDataInstance);
        Debug.Log("Attempting to save settings file to " + Application.persistentDataPath + "/settings" + ".set");
        var fileStream = File.Create(Application.persistentDataPath + "/settings" + ".set");
        jsonStream.Seek(0, SeekOrigin.Begin);
        jsonStream.CopyTo(fileStream);
        fileStream.Close();
    }

    public void LoadSettings()
    {
        SettingsData settingsDataInstance = GetDefaultSettingsData();
        var jsonSerializer = new DataContractJsonSerializer(
            typeof(SettingsData)
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
            settingsDataInstance = (SettingsData) jsonSerializer.ReadObject(fileStream);
        }
        catch (SerializationException e)
        {
            Debug.LogError(e);
            Debug.LogWarning("SAVEMANAGER - LOADSETTINGS: Something is wrong with the settings file, ignoring it.");
            LoadDefaultSettings();
            fileStream.Close();
            return;
        }

        currentSettingsData = settingsDataInstance;
        SetSettings(settingsDataInstance);
        fileStream.Close();
    }

    public void SetSettings(SettingsData settingsData)
    {
        currentSettingsData = settingsData;
        if (VolumeManager.Instance != null)
        {
            VolumeManager.Instance.ChangeMasterVolume(settingsData.masterVolume);
            VolumeManager.Instance.ChangeMusicVolume(settingsData.musicVolume);
            VolumeManager.Instance.ChangeSoundsVolume(settingsData.soundsVolume);
            VolumeManager.Instance.UpdateSliders();
        }
        else
        {
            Debug.LogWarning("SAVEMANAGER - LOADSETTINGS: Cannot access VolumeManager instance. (This is probably fine)");
        }
        
        if (SettingsResolution.Instance != null)
        {
            SettingsResolution.Instance.SetRes(settingsData.resolution);
        }
        else
        {
            Debug.LogWarning("SAVEMANAGER - LOADSETTINGS: Cannot access SettingsResolution instance.");
        }
    }
    
    private void LoadDefaultSettings()
    {
        SetSettings(GetDefaultSettingsData());
    }
    
    public void ResetSettings()
    {
        SaveSettings(GetDefaultSettingsData());
    }
    
    public SettingsData GetDefaultSettingsData()
    {
        SettingsData settingsDataInstance = new SettingsData
        {
            masterVolume = 0.5f,
            musicVolume = 0.5f,
            soundsVolume = 0.5f,
            resolution = global::SettingsData.Resolution1
        };
        return settingsDataInstance;
    }
    
}