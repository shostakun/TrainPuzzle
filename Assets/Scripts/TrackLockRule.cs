using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Diagnostics;

public class TrackLockRule : MonoBehaviour, ToolLockRule
{
    public bool IsLocked(Track track)
    {
        if (ToolManager.inst.currentObj != null)
        {
            List<string> toolPaths = Track.GetPathList(ToolManager.inst.currentObj);
            string toolPathString = Utils.RemoveDups(string.Join("", toolPaths));
            Vector2Int addr = Board.inst.ToAddress(track.transform.position);
            if (toolPathString.Contains("N") && addr.y >= Board.inst.height - 1) return true;
            if (toolPathString.Contains("S") && addr.y <= 0) return true;
            if (toolPathString.Contains("E") && addr.x >= Board.inst.width - 1) return true;
            if (toolPathString.Contains("W") && addr.x <= 0) return true;
            List<string> existingPaths = Track.GetPathList(track.gameObject);
            if (existingPaths.Intersect(toolPaths).Count() > 0) return true;
            foreach (char dir in toolPathString)
            {
                Track neighbor = Board.inst.GetNeighbor(track, dir.ToString());
                if (neighbor == null) continue;
                if (neighbor.GetObstacleDirections().Contains(Board.GetOpposite(dir.ToString()))) return true;
            }
        }
        return false;
    }
}
