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
    public class SaveData
    {
        [DataMember] 
        public DayInfo dayInfo;
    }

    public void SaveToFile(SaveData saveDataOverride = null, int saveNum = 1)
    {
        SaveData saveDataInstance = GetDefaultSaveData();

        if (saveDataOverride != null)
        {
            saveDataInstance = saveDataOverride;
        }
        else
        {
            if (DayManager.Instance != null)
            {
                saveDataInstance.dayInfo = DayManager.Instance.dayInfo;
            }
            else
            {
                Debug.LogWarning(
                    "SAVEMANAGER - SAVETOFILE: Cannot access DayManager instance."
                );
            }
        }
        
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
        SaveData saveDataInstance = GetDefaultSaveData();
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
            Debug.LogWarning(
                "SAVEMANAGER - LOADFROMFILE: Serialization Error: Something is wrong with the read save file, using default data."
            );
            LoadDefaultSave();
            fileStream.Close();
            return;
        }
        
        SetCurrentSaveData(saveDataInstance);
        fileStream.Close();
    }

    private void SetCurrentSaveData(SaveData saveDataInstance)
    {
        currentSaveData = saveDataInstance;
        if (DayManager.Instance != null)
        {
            DayManager.Instance.dayInfo = saveDataInstance.dayInfo;
            currentSaveData = saveDataInstance;
        }
        else
        {
            currentSaveData = GetDefaultSaveData();
            Debug.LogWarning(
                "SAVEMANAGER - LOADFROMFILE: Cannot access DayManager instance."
            );
        }
    }

    private void LoadDefaultSave()
    {
        SetCurrentSaveData(GetDefaultSaveData());
    }
    
    public void ResetSaveData()
    {
        SaveToFile(GetDefaultSaveData());
    }
    
    public SaveData GetDefaultSaveData()
    {
        SaveData saveDataInstance = new SaveData
        {
            dayInfo = new DayInfo()
        };
        
        saveDataInstance.dayInfo.statA = 30;
        saveDataInstance.dayInfo.statB = 30;
        saveDataInstance.dayInfo.statC = 30;

        saveDataInstance.dayInfo.sinkA = -15;
        saveDataInstance.dayInfo.sinkB = -15;
        saveDataInstance.dayInfo.sinkC = -15;

        saveDataInstance.dayInfo.day = 1;
        saveDataInstance.dayInfo.impeached = false;
        saveDataInstance.dayInfo.lose = false;

        return saveDataInstance;
    }
        
}
