using System.Collections.Generic;
using NaughtyAttributes;
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

public class SaveData : SaveFileBase
{
    public static SaveData inst;

    protected override string filePath => SaveFileManager.inst.GetCurrentFilePath();

    public BoardSize boardSize;
    [ReadOnly]
    public List<RockSettings> rockSettings;
    [ReadOnly]
    public List<StationSettings> stationSettings;
    [ReadOnly]
    public List<TrackSettings> trackSettings;

    protected Screenshot screenshot;

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
        screenshot = GetComponent<Screenshot>();
    }

    protected override void AfterLoad()
    {
        Board.inst.Initialize(boardSize);
    }

    protected override void AfterSave()
    {
        if (screenshot) screenshot.Take(SaveFileManager.inst.GetCurrentFilePath("png"));
    }

    protected override void BeforeSave()
    {
        Clear();
        foreach (Savable savable in GetComponentsInChildren<Savable>())
        {
            savable.UpdateSaveData(this);
        }
        SaveFileManager.inst.Save();
    }

    public void Clear()
    {
        boardSize = BoardSize.Medium;
        rockSettings = new List<RockSettings>();
        stationSettings = new List<StationSettings>();
        trackSettings = new List<TrackSettings>();
    }
}
