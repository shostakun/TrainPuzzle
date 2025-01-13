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

    void HandleFingerTap(LeanFinger finger)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(finger.ScreenPosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
            {
                TileHighlight highlight = hit.collider.GetComponent<TileHighlight>();
                if (highlight != null) highlight.PlaceObject();
            }
        }
    }
}
