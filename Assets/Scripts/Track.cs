using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Track : MonoBehaviour
{
    public bool isInUse = false;
    public bool isStation = false;
    public bool isLocked => isStation || isInUse;
    public UnityAction onTrainEnter;
    public UnityAction onTrainExit;

    [SerializeField]
    private bool _isTool = false;
    public bool isTool
    {
        get { return _isTool; }
        set
        {
            _isTool = value;
            transform.localScale = new Vector3(baseScale, baseScale, baseScale);
            if (_isTool) transform.localScale *= toolScaleFactor;
        }
    }

    protected float baseScale;
    public float toolScaleFactor = 1.25f;
    public float dragScaleFactor = 1.5f;

    void Awake()
    {
        baseScale = transform.localScale.x;
    }

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
