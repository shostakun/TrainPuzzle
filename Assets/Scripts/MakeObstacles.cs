using UnityEngine;

public class MakeObstacles : Initializer
{
    public GameObject[] obstaclePrefabs;

    public override void Initialize()
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
            GameObject obstacle = Instantiate(obstaclePrefab, Board.inst.ToPosition(addr),
                Quaternion.identity, tile.transform);
            obstacle.name = obstaclePrefab.name;
            i++;
        }
    }
}
