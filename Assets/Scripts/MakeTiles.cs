using UnityEngine;

public class MakeTiles : MonoBehaviour
{
    public GameObject tilePrefab;

    void Start()
    {
        int width = Board.inst.width;
        int height = Board.inst.height;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Instantiate(tilePrefab, transform.position + new Vector3(x, 0, z), Quaternion.identity, transform);
            }
        }
    }
}
