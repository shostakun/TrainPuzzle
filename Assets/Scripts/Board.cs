using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform tileContainer;
    public GameObject tilePrefab;
    public int width = 10;
    public int height = 8;
    public Vector2Int highlight { get; protected set; } = new Vector2Int(-1, -1);
    private Tile[] tiles;

    void Awake()
    {
        tiles = new Tile[width * height];
        Camera.main.transform.position = new Vector3((width - 1f) / 2f, (height - 1f) / 2f, Camera.main.transform.position.z);
    }

    public Tile GetNeighbor(Tile tile, string direction)
    {
        Vector2Int address = ToAddress(tile.transform.position);
        switch (direction)
        {
            case "N":
                address.y++;
                break;
            case "S":
                address.y--;
                break;
            case "E":
                address.x++;
                break;
            case "W":
                address.x--;
                break;
        }
        return IsInside(address) ? tiles[ToIndex(address)] : null;
    }

    public static string GetOpposite(string direction)
    {
        switch (direction)
        {
            case "N": return "S";
            case "S": return "N";
            case "E": return "W";
            case "W": return "E";
            default: return "";
        }
    }

    public void Highlight(Vector3 position)
    {
        highlight = IsInside(position) ?
            new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)) :
            new Vector2Int(-1, -1);
    }

    public bool IsInside(Vector2Int address)
    {
        return address.x >= 0 && address.x < width && address.y >= 0 && address.y < height;
    }

    public bool IsInside(Vector3 position)
    {
        return IsInside(ToAddress(position));
    }

    public void RemoveTile(Tile tile)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] == tile)
            {
                tiles[i] = null;
                break;
            }
        }
    }

    public void SetTile(Tile tile)
    {
        Vector3 position = SnapToGrid(tile.transform.position);
        if (IsInside(position))
        {
            int index = ToIndex(position);
            Tile oldTile = tiles[index];
            if (oldTile != null)
            {
                if (oldTile.isLocked)
                {
                    tile.Erase();
                    return;
                }
                tiles[index].Erase();
            }
            tiles[index] = tile;
        }
    }

    public Vector3 SnapToGrid(Vector3 position)
    {
        return new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), transform.position.z);
    }

    public Vector2Int ToAddress(int index)
    {
        return new Vector2Int(index % width, index / width);
    }

    public Vector2Int ToAddress(Vector3 position)
    {
        return new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
    }

    public int ToIndex(Vector2Int address)
    {
        return address.x + address.y * width;
    }

    public int ToIndex(Vector3 position)
    {
        return ToIndex(ToAddress(position));
    }

    public Vector3 ToPosition(Vector2Int address)
    {
        return new Vector3(address.x, address.y, transform.position.z);
    }

    public Vector3 ToPosition(int index)
    {
        return ToPosition(ToAddress(index));
    }
}
