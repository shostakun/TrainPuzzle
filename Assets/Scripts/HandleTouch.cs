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
        if (InputManager.inst.isLocked) return;
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Vector3? worldPosition = CameraUtil.GetWorldPosition(finger.ScreenPosition);
            if (worldPosition.HasValue)
            {
                TileHighlight highlight = Board.inst.GetTrack(worldPosition.Value)?.GetComponent<TileHighlight>();
                if (highlight != null) highlight.PlaceObject();
            }
        }
    }
}
