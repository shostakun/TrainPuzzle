using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Savable : MonoBehaviour
{
    protected void Save()
    {
        SaveData.inst.Save();
    }

    public virtual void UpdateSaveData(SaveData saveData)
    {

    }
}

public class SaveData : MonoBehaviour
{
    public static SaveData inst;

    protected string fileName = "/saveData.json";

    public BoardSize boardSize;
    public List<RockSettings> rockSettings;
    public List<StationSettings> stationSettings;
    public List<TrackSettings> trackSettings;

    void Awake()
    {
        if (inst == null)
        {
            inst = this;
            Clear();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Load();
    }

    public void Clear()
    {
        boardSize = BoardSize.Medium;
        rockSettings = new List<RockSettings>();
        stationSettings = new List<StationSettings>();
        trackSettings = new List<TrackSettings>();
    }

    protected void Initialize()
    {
        Board.inst.Initialize(boardSize);
    }

    protected void Load()
    {
        try
        {
            string filepath = Application.persistentDataPath + fileName;
            if (File.Exists(filepath))
            {
                using (StreamReader sr = new StreamReader(Application.persistentDataPath + fileName))
                {
                    JsonUtility.FromJsonOverwrite(sr.ReadToEnd(), this);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        Initialize();
    }

    public void Save()
    {
        Clear();
        foreach (Savable savable in GetComponentsInChildren<Savable>())
        {
            savable.UpdateSaveData(this);
        }

        try
        {
            using (StreamWriter sw = new StreamWriter(Application.persistentDataPath + fileName))
            {
                sw.Write(JsonUtility.ToJson(this));
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
