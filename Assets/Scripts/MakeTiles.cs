using UnityEngine;

public class MakeTiles : Initializer
{
    public GameObject tilePrefab;

    public override void Initialize()
    {
        int width = Board.inst.width;
        int height = Board.inst.height;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject go = Instantiate(tilePrefab, transform.position + new Vector3(x, 0, z),
                    Quaternion.identity, transform);
                go.name = $"Tile ({x}, {z})";
                Board.inst.SetTrack(go.GetComponent<Track>());
            }
        }
    }
}
