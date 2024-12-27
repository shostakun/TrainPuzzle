using System.Collections.Generic;
using UnityEngine;

public class TrackPath : MonoBehaviour
{
    public string start => path.Count > 1 ? path[0].name : "C";
    public string end => path.Count > 0 ? path[path.Count - 1].name : "C";

    public List<Transform> path;

    public Transform GetNext(Transform current = null)
    {
        int index = current == null ? -1 : path.IndexOf(current);
        return index < path.Count - 1 ? path[index + 1] : null;
    }
}
