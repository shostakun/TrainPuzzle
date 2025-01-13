using System;
using UnityEngine;

[Serializable]
public struct TrackSettings
{
    public Vector2Int addr;
    public string prefabName;
}

public class SaveTrack : Savable
{
    public override void UpdateSaveData(SaveData saveData)
    {
        saveData.trackSettings.Add(new TrackSettings
        {
            addr = Board.inst.ToAddress(transform.position),
            prefabName = gameObject.name
        });
    }
}
