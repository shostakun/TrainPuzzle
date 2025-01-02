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
            if (ToolManager.inst.IsLocked(this)) return true;
            return false;
        }
    }
    public UnityAction onTrainEnter;
    public UnityAction onTrainExit;

    public void Erase()
    {
        StartCoroutine(Shrink(Vector3.zero, transform.position, () => Destroy(gameObject)));
    }

    public List<string> GetObstacleDirections()
    {
        List<string> result = new List<string>();
        foreach (Obstacle obstacle in GetComponentsInChildren<Obstacle>())
        {
            if (obstacle.N) result.Add("N");
            if (obstacle.S) result.Add("S");
            if (obstacle.E) result.Add("E");
            if (obstacle.W) result.Add("W");
        }
        return result;
    }

    public TrackPath GetPathFrom(string start)
    {
        var paths = GetComponentsInChildren<TrackPath>().Where(p => p.start == start).ToList();
        if (paths.Count == 0) return null;
        return paths[Random.Range(0, paths.Count)];
    }

    public static List<string> GetPathList(GameObject obj)
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
