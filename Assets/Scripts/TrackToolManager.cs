using UnityEngine;
using UnityEngine.Events;

public class TrackToolManager : MonoBehaviour
{
    public static TrackToolManager inst { get; private set; }

    protected GameObject activeTile;
    protected GameObject currentObj;
    private string _currentTool = "";
    public string currentTool
    {
        get
        {
            return _currentTool;
        }
        protected set
        {
            bool changed = _currentTool != value;
            _currentTool = value;
            if (changed) onToolChange?.Invoke(value);
        }
    }

    public UnityAction<GameObject> onActiveTileChange;
    public UnityAction<string> onToolChange;
    public Transform trackContainer;

    void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ClearTool()
    {
        activeTile = null;
        onActiveTileChange?.Invoke(null);
        if (currentObj != null)
        {
            Destroy(currentObj);
        }
        currentTool = "";
    }

    public void PlaceObject()
    {
        if (activeTile == null || currentObj == null) return;
        Track track = Board.inst.GetTrack(currentObj.transform.position);
        if (track == null)
        {
            Debug.LogError("No track at " + currentObj.transform.position);
            return;
        }
        // TODO: Check board rules.
        Instantiate(currentObj, currentObj.transform.position, currentObj.transform.rotation, track.transform);
    }

    public void SetActiveTile(GameObject tile)
    {
        activeTile = tile;
        onActiveTileChange?.Invoke(tile);
        if (currentObj == null) return;
        currentObj.SetActive(tile != null);
        if (tile == null) return;
        currentObj.transform.position = tile.transform.position;
    }

    public void SetTool(ActivateTool tool)
    {
        if (currentObj != null)
        {
            Destroy(currentObj);
        }
        currentObj = Instantiate(tool.trackPrefab, trackContainer);
        currentObj.name = tool.trackPrefab.name;
        currentTool = tool.name;
    }
}
