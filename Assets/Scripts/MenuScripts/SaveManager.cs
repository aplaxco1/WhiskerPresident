using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

// NOTE: Used parts of my other Unity project's save/load code here -Justin

public class SaveManager : MonoBehaviour
{
    private static SaveData saveDataInstance = new SaveData();
    private static SettingsData settingsDataInstance = new SettingsData();
    public static SaveManager Instance;

    public SaveData currentSaveData;
    public SettingsData currentSettingsData;

    [Serializable]
    public class SaveData
    {
        [DataMember] 
        public DayInfo dayInfo;
    }
    
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
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            DayManager.Instance.InitializeDefaultStats();
        }
    }

    public void SaveSettings()
    {
        float masterVolumeToSave = 0.5f;
        float musicVolumeToSave = 0.5f;
        float soundsVolumeToSave = 0.5f;
        global::SettingsData.Resolution resToSave = global::SettingsData.Resolution1;

        if (VolumeManager.Instance != null)
        {
            masterVolumeToSave = VolumeManager.Instance.currentMasterVolume;
            musicVolumeToSave = VolumeManager.Instance.currentMusicVolume;
            soundsVolumeToSave = VolumeManager.Instance.currentSoundsVolume;
        }
        else
        {
            Debug.LogWarning("SAVEMANAGER - SAVESETTINGS: Cannot access VolumeManager instance.");
        }
        
        if (SettingsResolution.Instance != null)
        {
            resToSave = SettingsResolution.Instance.currentResolution;
        }
        else
        {
            Debug.LogWarning("SAVEMANAGER - SAVESETTINGS: Cannot access SettingsResolution instance.");
        }
        
        settingsDataInstance = new SettingsData
        {
            // update stuff here, then save to object file
            masterVolume = masterVolumeToSave,
            musicVolume = musicVolumeToSave,
            soundsVolume = soundsVolumeToSave,
            resolution = resToSave,
        };

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
            Debug.LogError("SAVEMANAGER - LOADSETTINGS: Something is wrong with the settings file, ignoring it.");
            LoadDefaultSettings();
            return;
        }

        currentSettingsData = settingsDataInstance;

        if (VolumeManager.Instance != null)
        {
            VolumeManager.Instance.ChangeMasterVolume(settingsDataInstance.masterVolume);
            VolumeManager.Instance.ChangeMusicVolume(settingsDataInstance.musicVolume);
            VolumeManager.Instance.ChangeSoundsVolume(settingsDataInstance.soundsVolume);
            VolumeManager.Instance.UpdateSliders();
        }
        else
        {
            Debug.LogWarning("SAVEMANAGER - LOADSETTINGS: Cannot access VolumeManager instance. (This is probably fine)");
        }
        
        if (SettingsResolution.Instance != null)
        {
            SettingsResolution.Instance.SetRes(settingsDataInstance.resolution);
        }
        else
        {
            Debug.LogWarning("SAVEMANAGER - LOADSETTINGS: Cannot access SettingsResolution instance.");
        }
    }

    public void SaveToFile(int saveNum = 1)
    {
        saveDataInstance = new SaveData
        {
            dayInfo = DayManager.Instance.dayInfo,
        };
        
        // write to file
        var jsonSerializer = new DataContractJsonSerializer(
            typeof(SaveData)
        );
        var jsonStream = new MemoryStream();
        jsonSerializer.WriteObject(jsonStream, saveDataInstance);
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
        catch (SerializationException se)
        {
            Debug.LogError(
                "SAVEMANAGER - LOADFROMFILE: Serialization error while loading save file, using default save data."
                + saveNum
                + ": "
                + se.Message
            );
            LoadDefaultSave();
            return;
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
            saveDataInstance = (SaveData) jsonSerializer.ReadObject(fileStream);
        }
        catch (SerializationException e)
        {
            Debug.LogError(e);
            Debug.LogError(
                "SAVEMANAGER - LOADFROMFILE: Serialization Error: Something is wrong with the read save file, using default data."
            );
            LoadDefaultSave();
            return;
        }

        currentSaveData = saveDataInstance;
        
        if (DayManager.Instance != null)
        {
            DayManager.Instance.dayInfo = saveDataInstance.dayInfo;
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
        saveDataInstance = new SaveData
        {
            dayInfo = new DayInfo()
        };
        
        currentSaveData = saveDataInstance;

        if (DayManager.Instance != null)
        {
            DayManager.Instance.dayInfo = saveDataInstance.dayInfo;
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
        settingsDataInstance = new SettingsData
        {
            masterVolume = 0.5f,
            musicVolume = 1.0f,
            soundsVolume = 1.0f,
            resolution = global::SettingsData.Resolution1
        };
    }


}