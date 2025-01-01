using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Track))]
public class TileHighlight : MonoBehaviour
{
    public float animationDuration = 0.2f;
    protected Material baseMaterial;
    public Material highlightMaterial;
    public Material lockedMaterial;
    public new Renderer renderer;
    protected Track track;

    void Start()
    {
        baseMaterial = renderer.material;
        TrackToolManager.inst.onActiveTileChange += SetHighlight;
        TrackToolManager.inst.onToolChange += SetHighlight;
        track = GetComponent<Track>();
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

    void SetHighlight()
    {
        if (TrackToolManager.inst.currentTool != "" && track.isLocked)
        {
            renderer.material = lockedMaterial;
        }
        else if (TrackToolManager.inst.currentTool != "" && TrackToolManager.inst.activeTile == gameObject)
        {
            renderer.material = highlightMaterial;
        }
        else
        {
            renderer.material = baseMaterial;
        }
    }

    void SetHighlight(GameObject tile)
    {
        SetHighlight();
    }

    void SetHighlight(string tool)
    {
        SetHighlight();
    }
}
