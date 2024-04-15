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
    private static SaveData _instance = new SaveData();
    private static Settings _settingsInst = new Settings();
    

    private SaveData()
    {
    }

    public static void SaveSettings()
    {
        _settingsInst = new Settings
        {
            // update stuff here, then save to object file
            // volume = AudioSlider.instance.slider.value
        };

        // write to file
        var jsonSerializer = new DataContractJsonSerializer(
            typeof(Settings)
        );
        var jsonStream = new MemoryStream();
        jsonSerializer.WriteObject(jsonStream, _settingsInst);
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
            _settingsInst = new Settings();
            _settingsInst.volume = .5f;
            // if (GameManager.instance.isMenu) AudioSlider.instance.slider.value = _settingsInst.volume;
            // AudioManager.instance.UpdateVolume(_settingsInst.volume);
            // Screen.fullScreen = _settingsInst.isFullscreen;
            return;
        }

        try
        {
            _settingsInst = (Settings) jsonSerializer.ReadObject(fileStream);
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
        _instance = new SaveData
        {
            // update stuff here, then save to object file
            // levelProgress = GameManager.instance.levelProgress
        };

        // write to file
        var jsonSerializer = new DataContractJsonSerializer(
            typeof(SaveData)
        );
        var jsonStream = new MemoryStream();
        jsonSerializer.WriteObject(jsonStream, _instance);
        var fileStream = File.Create(
            Application.persistentDataPath + "/save" + saveNum + ".pbb"
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
            fileStream = File.OpenRead(Application.persistentDataPath + "/save" + saveNum + ".pbb");
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
            _instance = new SaveData();
            // GameManager.instance.levelProgress = _instance.levelProgress;
            return;
        }

        try
        {
            _instance = (SaveData) jsonSerializer.ReadObject(fileStream);
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
        public float volume;
    }
}