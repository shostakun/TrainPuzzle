using Lean.Touch;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandleTouch : MonoBehaviour
{
    void OnEnable()
    {
        LeanTouch.OnFingerTap += HandleFingerTap;
    }

    void OnDisable()
    {
        LeanTouch.OnFingerTap -= HandleFingerTap;
    }

    static void HandleFingerTap(LeanFinger finger)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Vector3? worldPosition = GetWorldPosition(finger.ScreenPosition);
            if (worldPosition.HasValue)
            {
                TileHighlight highlight = Board.inst.GetTrack(worldPosition.Value)?.GetComponent<TileHighlight>();
                if (highlight != null) highlight.PlaceObject();
            }
        }
    }

    public static Vector3? GetWorldPosition(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        if (plane.Raycast(ray, out float distance)) return ray.GetPoint(distance);
        return null;
    }
}
