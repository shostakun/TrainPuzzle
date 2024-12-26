using UnityEngine;

public class MakeToolbox : MonoBehaviour
{
    public Board board;
    public int rowSize = 6;
    public GameObject[] tilePrefabs;

    void Start()
    {
        if (board == null) board = FindFirstObjectByType<Board>();
        float xOffset = 0;
        float yOffset = 0;
        for (int i = 0; i < tilePrefabs.Length; i++)
        {
            GameObject go = Instantiate(tilePrefabs[i], transform.position + new Vector3(xOffset, yOffset, 0), Quaternion.identity, transform);
            Tile tile = go.GetComponent<Tile>();
            tile.isTool = true;
            if (i % rowSize == rowSize - 1)
            {
                xOffset -= tile.toolScaleFactor;
                yOffset = 0;
            }
            else
                yOffset += tile.toolScaleFactor;
        }
    }
}
