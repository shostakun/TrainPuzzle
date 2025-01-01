using UnityEngine;

[RequireComponent(typeof(Track))]
public class TileHighlight : MonoBehaviour
{
    public GameObject highlight;
    protected Track track;

    void Start()
    {
        TrackToolManager.inst.onActiveTileChange += OnActiveTileChanged;
        track = GetComponent<Track>();
    }

    void OnActiveTileChanged(GameObject tile)
    {
        highlight.SetActive(tile == gameObject && !track.isLocked);
    }

    void OnMouseEnter()
    {
        TrackToolManager.inst.SetActiveTile(gameObject);
    }

    void OnMouseExit()
    {
        TrackToolManager.inst.SetActiveTile(null);
    }

    void OnMouseUpAsButton()
    {
        TrackToolManager.inst.PlaceObject();
    }
}
