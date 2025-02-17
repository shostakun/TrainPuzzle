using UnityEngine;
using UnityEngine.EventSystems;

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
        ToolManager.inst.onActiveTileChange += SetHighlight;
        ToolManager.inst.onToolChange += SetHighlight;
        track = GetComponent<Track>();
    }

    void OnDestroy()
    {
        ToolManager.inst.onActiveTileChange -= SetHighlight;
        ToolManager.inst.onToolChange -= SetHighlight;
    }

    void OnMouseEnter()
    {
        if (InputManager.inst.isLocked) return;
        ToolManager.inst.SetActiveTile(gameObject);
    }

    void OnMouseExit()
    {
        if (InputManager.inst.isLocked) return;
        ToolManager.inst.SetActiveTile(null);
    }

    public void PlaceObject()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        ToolManager.inst.PlaceObject();
    }

    void SetHighlight()
    {
        if (ToolManager.inst.currentTool != "" && track.isLocked)
        {
            renderer.material = lockedMaterial;
        }
        else if (ToolManager.inst.currentTool != "" && ToolManager.inst.activeTile == gameObject)
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
