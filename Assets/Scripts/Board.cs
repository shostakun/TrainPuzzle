using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board inst { get; private set; }

    public int width = 10;
    public int height = 8;
    public Vector2Int highlight { get; protected set; } = new Vector2Int(-1, -1);
    private Track[] tracks;

    protected void Awake()
    {
        if (inst == null)
        {
            inst = this;
            tracks = new Track[width * height];
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Track GetNeighbor(Track track, string direction)
    {
        Vector2Int address = ToAddress(track.transform.position);
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
            default:
                return null;
        }
        return IsInside(address) ? tracks[ToIndex(address)] : null;
    }

    public Track GetTrack(Vector3 position)
    {
        return IsInside(position) ? tracks[ToIndex(position)] : null;
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

    public bool IsEdge(Vector2Int address)
    {
        return address.x == 0 || address.x == width - 1 || address.y == 0 || address.y == height - 1;
    }

    public bool IsEdge(Vector3 position)
    {
        return IsEdge(ToAddress(position));
    }

    public bool IsInside(Vector2Int address)
    {
        return address.x >= 0 && address.x < width && address.y >= 0 && address.y < height;
    }

    public bool IsInside(Vector3 position)
    {
        return IsInside(ToAddress(position));
    }

    public void RemoveTrack(Track track)
    {
        for (int i = 0; i < tracks.Length; i++)
        {
            if (tracks[i] == track)
            {
                tracks[i] = null;
                break;
            }
        }
    }

    public void SetTrack(Track track)
    {
        Vector3 position = SnapToGrid(track.transform.position);
        if (IsInside(position))
        {
            int index = ToIndex(position);
            Track oldTrack = tracks[index];
            if (oldTrack != null)
            {
                if (oldTrack.isLocked)
                {
                    track.Erase();
                    return;
                }
                tracks[index].Erase();
            }
            tracks[index] = track;
        }
    }

    public Vector3 SnapToGrid(Vector3 position)
    {
        return ToPosition(ToAddress(position));
    }

    public Vector2Int ToAddress(int index)
    {
        return new Vector2Int(index % width, index / width);
    }

    public Vector2Int ToAddress(Vector3 position)
    {
        return new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
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
        return new Vector3(address.x, transform.position.y, address.y);
    }

    public Vector3 ToPosition(int index)
    {
        return ToPosition(ToAddress(index));
    }
}
