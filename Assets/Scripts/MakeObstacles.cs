using UnityEngine;

public class MakeObstacles : Initializer
{
    public GameObject[] obstaclePrefabs;

    public override void Initialize()
    {
        bool loaded = LoadRocks();
        // loaded = LoadTrees() || loaded; // For future use.
        if (!loaded) MakeNewObstacles();
    }

    bool LoadRocks()
    {
        if (SaveData.inst.rockSettings.Count < 1) return false;

        // Find the array of rock prefabs.
        GameObject rockPrefab = null;
        GameObject[] rocks = null;
        foreach (GameObject prefab in obstaclePrefabs)
        {
            if (prefab.name == "Rock")
            {
                rockPrefab = prefab;
                var ri = prefab.GetComponent<RandomInstance>();
                if (ri != null) rocks = ri.prefabs;
                break;
            }
        }
        if (rockPrefab == null || rocks == null)
        {
            Debug.LogError("Rock prefabs not found.");
            return false;
        }

        // Load the rocks.
        foreach (RockSettings rockSettings in SaveData.inst.rockSettings)
        {
            foreach (GameObject prefab in rocks)
            {
                if (prefab.name == rockSettings.prefabName)
                {
                    Track tile = Board.inst.GetTrack(rockSettings.addr);
                    GameObject obstacle = tile.AddInstance(rockPrefab);
                    RandomInstance ri = obstacle.GetComponent<RandomInstance>();
                    if (ri != null) ri.RefreshWithPrefab(prefab, rockSettings.rotation);
                    break;
                }
            }
        }
        return true;
    }

    void MakeNewObstacles()
    {
        int number = Random.Range(Board.inst.settings.minObstacles, Board.inst.settings.maxObstacles + 1);
        for (int i = 0; i < number;)
        {
            GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            Vector2Int addr = new Vector2Int(
                Random.Range(1, Board.inst.width - 1),
                Random.Range(1, Board.inst.height - 1));
            Track tile = Board.inst.GetTrack(addr);
            if (tile == null || ObstacleLockRule.CheckRule(tile)) continue;
            tile.AddInstance(obstaclePrefab);
            i++;
        }
    }
}
