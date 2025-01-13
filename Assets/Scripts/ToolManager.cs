using UnityEngine;
using UnityEngine.Events;

public interface ToolLockRule
{
    public bool IsLocked(Track track);
}

public interface ToolRefresher
{
    public void Refresh();
}

public class ToolManager : MonoBehaviour
{
    public static ToolManager inst { get; private set; }

    public GameObject activeTile { get; protected set; }
    public GameObject currentObj { get; protected set; }
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
    protected ToolLockRule[] lockRules;
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

    void Start()
    {
        Board.inst.onInitialized += HandleInitialized;
    }

    void OnDestroy()
    {
        Board.inst.onInitialized -= HandleInitialized;
    }

    public void ClearTool()
    {
        activeTile = null;
        onActiveTileChange?.Invoke(null);
        if (currentObj != null)
        {
            Destroy(currentObj);
        }
        lockRules = new ToolLockRule[0];
        currentTool = "";
    }

    void HandleInitialized(bool initialized)
    {
        if (initialized) ClearTool();
    }

    public bool IsLocked(Track track)
    {
        foreach (ToolLockRule rule in lockRules)
        {
            if (rule.IsLocked(track)) return true;
        }
        return false;
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
        if (!track.isLocked)
            Instantiate(currentObj, currentObj.transform.position, currentObj.transform.rotation, track.transform);
        foreach (ToolRefresher refresher in currentObj.GetComponentsInChildren<ToolRefresher>())
        {
            refresher.Refresh();
        }
    }

    public void SetActiveTile(GameObject tile)
    {
        activeTile = tile;
        onActiveTileChange?.Invoke(tile);
        if (currentObj == null) return;
        if (tile == null)
        {
            currentObj.SetActive(false);
            return;
        }
        Track track = Board.inst.GetTrack(tile.transform.position);
        currentObj.SetActive(track != null && !track.isLocked);
        if (tile == null) return;
        currentObj.transform.position = tile.transform.position;
    }

    public void SetTool(ActivateTool tool)
    {
        if (currentObj != null)
        {
            Destroy(currentObj);
        }
        currentObj = Instantiate(tool.prefab, trackContainer);
        currentObj.name = tool.prefab.name;
        currentObj.SetActive(false);
        lockRules = tool.GetComponentsInChildren<ToolLockRule>();
        if (tool.iconImageObject != null)
        {
            Deletable deletable = currentObj.GetComponentInChildren<Deletable>();
            if (deletable != null)
            {
                deletable.iconPrefab = tool.iconImageObject;
            }
        }
        currentTool = tool.name;
    }
}
