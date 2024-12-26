using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    public bool N = false;
    public bool S = false;
    public bool E = true;
    public bool W = true;
    public Dictionary<string, Vector3> joints = new Dictionary<string, Vector3>();
    public bool isInUse = false;
    public bool isStation = false;
    public bool isLocked => isStation || isInUse;
    public float halfTile = 0.4f;

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

    private Board board;

    void Awake()
    {
        baseScale = transform.localScale.x;
        board = FindFirstObjectByType<Board>();
        SetJoints();
    }

    void OnMouseDown()
    {
        if (isTool)
        {
            GameObject go = Instantiate(gameObject, transform.position, transform.rotation, transform.parent);
            Tile tile = go.GetComponent<Tile>();
            tile.baseScale = baseScale;
        }
        else
        {
            board.RemoveTile(this);
        }
        isTool = false;
    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        // Debug.Log(mousePosition);
        transform.position = mousePosition;
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * baseScale * dragScaleFactor, 0.1f);
        board.Highlight(transform.position);
    }

    void OnMouseUp()
    {
        isTool = false;
        if (!board.IsInside(transform.position))
        {
            Erase();
            return;
        }
        StartCoroutine(Shrink(Vector3.one * baseScale, board.SnapToGrid(transform.position)));
        SetJoints();
        board.Highlight(Vector3.one * -1);
        board.SetTile(this);
    }

    public void Erase()
    {
        StartCoroutine(Shrink(Vector3.zero, transform.position, () => Destroy(gameObject)));
    }

    public TrackPath GetPathFrom(string start)
    {
        var paths = GetComponents<TrackPath>().Where(p => p.start == start).ToList();
        if (paths.Count == 0) return null;
        return paths[Random.Range(0, paths.Count)];
    }

    protected void SetJoints()
    {
        joints.Clear();
        Vector3 position = board.SnapToGrid(transform.position);
        if (N) joints.Add("N", position + Vector3.up * halfTile);
        if (S) joints.Add("S", position + Vector3.down * halfTile);
        if (E) joints.Add("E", position + Vector3.right * halfTile);
        if (W) joints.Add("W", position + Vector3.left * halfTile);
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
