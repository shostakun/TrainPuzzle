using System;
using UnityEngine;

[Serializable]
public struct RockSettings
{
    public Vector2Int addr;
    public string prefabName;
    public Quaternion rotation;
}

public class SaveRock : Savable
{
    public override void UpdateSaveData(SaveData saveData)
    {
        Transform rock = transform.GetChild(0);
        saveData.rockSettings.Add(new RockSettings
        {
            addr = Board.inst.ToAddress(transform.position),
            rotation = rock.rotation,
            prefabName = rock.name
        });
    }
}
