using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

// NOTE: Used parts of my other Unity project's save/load code here -Justin
[Serializable]
public class SaveData
{
    // this is the only place the constructor should ever be called
    private static SaveData instance = new SaveData();
    private static Settings settingsInst = new Settings();
    

    private SaveData()
    {
        StatVector statVector = new StatVector();
        statVector.RedStat = 50;
        statVector.GreenStat = 50;
        statVector.BlueStat = 50;
        int dayProgression = 0;
    }

    public static void SaveSettings()
    {
        settingsInst = new Settings
        {
            // update stuff here, then save to object file
            // volume = AudioSlider.instance.slider.value
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
                "IO ERROR WHILE LOADING FILE, CREATING DEFAULT SETTINGS OBJECT "
                + ": "
                + ioe.Message
            );

            //DEFAULT SETTINGS
            settingsInst = new Settings();
            return;
        }

        try
        {
            settingsInst = (Settings) jsonSerializer.ReadObject(fileStream);
        }
        catch (SerializationException)
        {
            Debug.LogError("something is wrong with the settings file, ignoring it");
            //DEFAULT SETTINGS
            return;
        }
        
        // AudioManager.instance.UpdateVolume(_settingsInst.volume);
    }

    public static void SaveToFile(int saveNum)
    {
        instance = new SaveData
        {
            // update stuff here, then save to object file
            // levelProgress = GameManager.instance.levelProgress
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
                "IO ERROR WHILE LOADING SAVEFILE, USING DEFAULT SAVE OBJECT "
                + saveNum
                + ": "
                + ioe.Message
            );
            //DEFAULT SAVE OBJECT
            instance = new SaveData();
            // GameManager.instance.levelProgress = _instance.levelProgress;
            return;
        }

        try
        {
            instance = (SaveData) jsonSerializer.ReadObject(fileStream);
        }
        catch (SerializationException)
        {
            // GameManager.instance.levelProgress = defaultLevelProgress;
            Debug.LogError(
                "something is wrong with the read save file, using default lvl progress"
            );
            return;
        }
    }

    public class Settings
    {
        public SettingsData.Volume volume;
        public SettingsData.Resolution resolution;

        public Settings()
        {
            // Default Settings Data
            volume = new SettingsData.Volume();
            volume.magnitude = 0.5f;
            resolution = SettingsData.Resolution1;
        }
    }
}