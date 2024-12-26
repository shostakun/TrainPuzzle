using UnityEngine;

public class MakeTiles : MonoBehaviour
{
    public GameObject tilePrefab;
    private Board board;

    void Start()
    {
        board = GetComponentInParent<Board>();
        int width = board.width;
        int height = board.height;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tile = Instantiate(tilePrefab, transform.position + new Vector3(x, y, 0), Quaternion.identity, transform);
            }
        }
    }
}
