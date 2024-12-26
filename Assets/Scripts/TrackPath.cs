using System.Collections.Generic;
using UnityEngine;

public class TrackPath : MonoBehaviour
{
    public string start = "E";
    public string end = "W";

    public List<Transform> path;

    public Transform GetNext(Transform current = null)
    {
        int index = current == null ? -1 : path.IndexOf(current);
        return index < path.Count - 1 ? path[index + 1] : null;
    }
}
