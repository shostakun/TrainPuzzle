using UnityEngine;

public class ObstacleLockRule : MonoBehaviour, ToolLockRule
{
    public bool IsLocked(Track track)
    {
        if (ToolManager.inst.currentObj != null)
        {
            return CheckRule(track);
        }
        return false;
    }

    public static bool CheckRule(Track track)
    {
        if (Board.inst.IsEdge(track.transform.position)) return true;
        if (track.GetComponentInChildren<TrackPath>() != null) return true;
        if (track.GetComponentInChildren<Obstacle>() != null) return true;
        string[] dirs = { "N", "S", "E", "W" };
        foreach (string dir in dirs)
        {
            Track neighbor = Board.inst.GetNeighbor(track, dir);
            if (neighbor != null && neighbor.GetPathFrom(Board.GetOpposite(dir)) != null) return true;
        }
        return false;
    }
}
