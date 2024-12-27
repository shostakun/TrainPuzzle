using UnityEngine;

public class TileHighlight : MonoBehaviour
{
    public GameObject highlight;

    void Start()
    {
        TrackToolManager.inst.onActiveTileChange += OnActiveTileChanged;
    }

    void OnActiveTileChanged(GameObject tile)
    {
        highlight.SetActive(tile == gameObject);
    }
}
