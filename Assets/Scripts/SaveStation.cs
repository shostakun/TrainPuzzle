using System;
using UnityEngine;

[Serializable]
public struct StationSettings
{
    public Vector2Int addr;
    public string prefabName;
}

public class SaveStation : Savable
{
    public override void UpdateSaveData(SaveData saveData)
    {
        saveData.stationSettings.Add(new StationSettings
        {
            addr = Board.inst.ToAddress(transform.position),
            prefabName = gameObject.name
        });
    }
}
