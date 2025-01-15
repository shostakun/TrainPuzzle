using UnityEngine;

public static class CameraUtil
{
    public static Vector3? GetWorldPosition(Vector2 screenPosition, Camera camera = null)
    {
        if (camera == null) camera = Camera.main;
        Ray ray = camera.ScreenPointToRay(screenPosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        if (plane.Raycast(ray, out float distance)) return ray.GetPoint(distance);
        return null;
    }
}
