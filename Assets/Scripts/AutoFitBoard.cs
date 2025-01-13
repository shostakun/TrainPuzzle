using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class AutoFitBoard : MonoBehaviour
{
    private bool center_ = true;
    public bool center
    {
        get => center_;
        set
        {
            center_ = value;
            if (centeringUI) centeringUI.SetActive(!value);
        }
    }
    public GameObject centeringUI;
    public float halfTileSize = 0.5f;
    public float menuOffset = 200;
    public float sensitivity = 0.05f;
    protected LeanFingerFilter Use = new LeanFingerFilter(true);

    protected Vector3 bottomLeft;
    protected Vector3 topRight;
    protected Vector3 topLeft;
    protected Vector3 bottomRight;

    void Start()
    {
        center = true;
        Board.inst.onInitialized += HandleInitialized;
        HandleInitialized(Board.inst.initialized);
    }

    void Update()
    {
        UpdateGesture();
        UpdateScroll();
        if (center) UpdateZoom();
    }

    void UpdateGesture()
    {
        List<LeanFinger> fingers = Use.UpdateAndGetFingers();
        if (fingers.Count == 1)
        {
            LeanFinger finger = fingers[0];
            if (finger.StartedOverGui) return;
            if (!finger.Tap)
            {
                Vector2 delta = finger.ScreenDelta;
                if (delta != Vector2.zero)
                {
                    center = false;
                    Camera.main.transform.position = TargetPositionForOffset(delta.x, delta.y);
                }
            }
            return;
        }
        float pinchScale = LeanGesture.GetPinchRatio(fingers);
        if (pinchScale != 1.0f)
        {
            center = false;
            ScaleAroundPoint(pinchScale, LeanGesture.GetScreenCenter(fingers));
        }
    }

    void UpdatePositions()
    {
        bottomLeft = Board.inst.GetTrack(new Vector2Int(0, 0))
            .transform.position - new Vector3(halfTileSize, 0, halfTileSize);
        topRight = Board.inst.GetTrack(new Vector2Int(Board.inst.width - 1, Board.inst.height - 1))
            .transform.position + new Vector3(halfTileSize, 0, halfTileSize);
        topLeft = new Vector3(bottomLeft.x, 0, topRight.z);
        bottomRight = new Vector3(topRight.x, 0, bottomLeft.z);
    }

    void UpdateScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            center = false;
            ScaleAroundPoint(1 - scroll, Input.mousePosition);
        }
    }

    void UpdateZoom()
    {
        // TODO: This is making assumptions about the camera's rotation, and I don't like that.
        Vector3 topRightScreen = Camera.main.WorldToScreenPoint(topRight);
        Vector3 topLeftScreen = Camera.main.WorldToScreenPoint(topLeft);
        float top = topRightScreen.y;
        float bottom = Camera.main.WorldToScreenPoint(bottomLeft).y;
        float left = topLeftScreen.x;
        float right = Camera.main.WorldToScreenPoint(bottomRight).x;

        float scaleRatio = Mathf.Max(
            (top - bottom) / Camera.main.pixelHeight,
            (right - left) / (Camera.main.pixelWidth - menuOffset));
        float targetSize = scaleRatio * Camera.main.orthographicSize;
        SetCameraSize(Mathf.Lerp(Camera.main.orthographicSize, targetSize, sensitivity));

        Vector3 targetPosition = TargetPositionForOffset(
            (Camera.main.pixelWidth - right - (left - menuOffset)) / 2,
            (Camera.main.pixelHeight - top - bottom) / 2);
        Camera.main.transform.position = Vector3.Lerp(
            Camera.main.transform.position, targetPosition, sensitivity);
    }

    public void EnableCentering(bool enable)
    {
        center = enable;
    }

    void HandleInitialized(bool initialized)
    {
        if (initialized) UpdatePositions();
    }

    void ScaleAroundPoint(float scale, Vector2 startPosition)
    {
        Vector3? worldPosition = HandleTouch.GetWorldPosition(startPosition);
        SetCameraSize(Camera.main.orthographicSize * scale);
        if (worldPosition.HasValue)
        {
            // Adjust the camera position so that the world position under the mouse stays the same.
            Vector2 endPosition = Camera.main.WorldToScreenPoint(worldPosition.Value);
            Vector2 offset = startPosition - endPosition;
            Camera.main.transform.position = TargetPositionForOffset(offset.x, offset.y);
        }
    }

    void SetCameraSize(float size)
    {
        Camera.main.orthographicSize = Mathf.Clamp(size, 0.5f, 30f);
    }

    Vector3 TargetPositionForOffset(float x, float y)
    {
        // Camera position so that the board is centered, leaving space on the left for the menu,
        // accounting for the rotation of the camera, and keeping the camera the same distance from the board.
        // TODO: This is making assumptions about the camera's rotation, and I don't like that.
        Vector3 topLeftScreen = Camera.main.WorldToScreenPoint(topLeft);
        Vector3 topRightScreen = Camera.main.WorldToScreenPoint(topRight);
        topLeftScreen.x += x;
        topRightScreen.y += y;
        Vector3 xOffset = topLeft - Camera.main.ScreenToWorldPoint(topLeftScreen);
        Vector3 yOffset = topRight - Camera.main.ScreenToWorldPoint(topRightScreen);
        return Camera.main.transform.position + xOffset + yOffset;
    }
}
