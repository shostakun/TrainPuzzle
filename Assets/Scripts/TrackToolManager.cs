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
            Debug.Log("Current tool: " + value);
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            currentTool = "";
        }
        if (currentTool == "") return;
        GetActiveTile();
        PlaceObject();
    }

    void GetActiveTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray);
        GameObject oldActiveTile = activeTile;
        if (hits.Length == 0)
        {
            activeTile = null;
        }
        else
        {
            RaycastHit hit = new RaycastHit();
            float minDistance = float.MaxValue;
            foreach (var h in hits)
            {
                if (h.distance < minDistance)
                {
                    minDistance = h.distance;
                    hit = h;
                }
            }
            activeTile = hit.collider.gameObject;
        }

        if (activeTile != oldActiveTile && onActiveTileChange != null) onActiveTileChange(activeTile);

        if (currentObj == null) return;
        if (activeTile == null)
        {
            currentObj.SetActive(false);
        }
        else
        {
            currentObj.SetActive(true);
            currentObj.transform.position = activeTile.transform.position;
        }
    }

    void PlaceObject()
    {
        if (activeTile == null || currentObj == null) return;
        if (Input.GetMouseButtonDown(0))
        {
            // TODO: Check board rules.
            GameObject newObj = Instantiate(currentObj, currentObj.transform.position, currentObj.transform.rotation, trackContainer);
            Track track = newObj.GetComponent<Track>();
            if (track != null) Board.inst.SetTrack(track);
        }
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
