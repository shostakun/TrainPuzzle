using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Track : MonoBehaviour
{
    public bool isInUse = false;
    public bool isStation = false;
    public bool isLocked
    {
        get
        {
            if (isInUse || isStation) return true;
            if (TrackToolManager.inst.currentObj != null)
            {
                List<string> toolPaths = GetPathList(TrackToolManager.inst.currentObj);
                string toolPathString = string.Join("", toolPaths);
                Vector2Int addr = Board.inst.ToAddress(transform.position);
                if (toolPathString.Contains("N") && addr.y == Board.inst.height - 1) return true;
                if (toolPathString.Contains("S") && addr.y == 0) return true;
                if (toolPathString.Contains("E") && addr.x == Board.inst.width - 1) return true;
                if (toolPathString.Contains("W") && addr.x == 0) return true;
                List<string> existingPaths = GetPathList(gameObject);
                if (existingPaths.Intersect(toolPaths).Count() > 0) return true;
            }
            return false;
        }
    }
    public UnityAction onTrainEnter;
    public UnityAction onTrainExit;

    public void Erase()
    {
        StartCoroutine(Shrink(Vector3.zero, transform.position, () => Destroy(gameObject)));
    }

    public TrackPath GetPathFrom(string start)
    {
        var paths = GetComponentsInChildren<TrackPath>().Where(p => p.start == start).ToList();
        if (paths.Count == 0) return null;
        return paths[Random.Range(0, paths.Count)];
    }

    static List<string> GetPathList(GameObject obj)
    {
        return obj.GetComponentsInChildren<TrackPath>().Select(p => p.start + p.end).ToList();
    }

    public IEnumerator Shrink(Vector3 targetScale, Vector3 targetPosition, UnityAction callback = null)
    {
        for (int i = 0; i < 20; i++)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 0.1f);
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
            yield return null;
        }
        transform.localScale = targetScale;
        transform.position = targetPosition;
        if (callback != null) callback.Invoke();
    }
}
