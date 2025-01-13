using UnityEngine;

public class LoadTracks : Initializer
{
    public GameObject[] trackPrefabs;

    public override void Initialize()
    {
        if (SaveData.inst.trackSettings.Count < 1) return;

        foreach (TrackSettings settings in SaveData.inst.trackSettings)
        {
            foreach (GameObject prefab in trackPrefabs)
            {
                if (prefab.name == settings.prefabName)
                {
                    Track tile = Board.inst.GetTrack(settings.addr);
                    tile.AddInstance(prefab);
                    break;
                }
            }
        }
    }
}
