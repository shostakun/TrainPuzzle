using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class AutoFitBoard : MonoBehaviour
{
    public Camera targetCamera;
    private bool center_ = true;
    public bool center
    {
        get => center_;
        set
        {
            center_ = value;
            if (centeringUI) centeringUI.SetActive(interactive && !value);
        }
    }
    public GameObject centeringUI;
    public float halfTileSize = 0.5f;
    public bool interactive = true;
    public bool matchAspectRatio = false;
    public float menuOffset = 200;
    public float sensitivity = 0.05f;
    protected LeanFingerFilter Use = new LeanFingerFilter(true);

    protected Vector3 bottomLeft;
    protected Vector3 topRight;
    protected Vector3 topLeft;
    protected Vector3 bottomRight;

    void Start()
    {
        if (targetCamera == null) targetCamera = GetComponent<Camera>();
        if (targetCamera == null) targetCamera = Camera.main;
        center = true;
        Board.inst.onInitialized += HandleInitialized;
        HandleInitialized(Board.inst.initialized);
    }

    void OnDestroy()
    {
        Board.inst.onInitialized -= HandleInitialized;
    }

    void Update()
    {
        if (!interactive) return;
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
            if (finger.TapCount == 2)
            {
                center = true;
            }
            else if (finger.ScreenDelta != Vector2.zero)
            {
                Vector2 delta = finger.ScreenDelta;
                if (delta != Vector2.zero)
                {
                    center = false;
                    targetCamera.transform.position = TargetPositionForOffset(delta.x, delta.y);
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
        if (center && !interactive) StartCoroutine(UpdateZoomDelayed());
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
        Vector3 topRightScreen = targetCamera.WorldToScreenPoint(topRight);
        Vector3 topLeftScreen = targetCamera.WorldToScreenPoint(topLeft);
        float top = topRightScreen.y;
        float bottom = targetCamera.WorldToScreenPoint(bottomLeft).y;
        float left = topLeftScreen.x;
        float right = targetCamera.WorldToScreenPoint(bottomRight).x;

        if (matchAspectRatio)
        {
            // TODO: Make this work...
            Debug.Log($"Current aspect ratio: {targetCamera.aspect} {(float)targetCamera.pixelWidth / (float)targetCamera.pixelHeight} {(right - left) / (top - bottom)}");
            targetCamera.aspect = (right - left) / (top - bottom);
        }

        float scaleRatio = Mathf.Max(
            (top - bottom) / targetCamera.pixelHeight,
            (right - left) / (targetCamera.pixelWidth - menuOffset));
        float targetSize = scaleRatio * targetCamera.orthographicSize;
        SetCameraSize(Mathf.Lerp(targetCamera.orthographicSize, targetSize, sensitivity));

        Vector3 targetPosition = TargetPositionForOffset(
            (targetCamera.pixelWidth - right - (left - menuOffset)) / 2,
            (targetCamera.pixelHeight - top - bottom) / 2);
        targetCamera.transform.position = Vector3.Lerp(
            targetCamera.transform.position, targetPosition, sensitivity);
    }

    IEnumerator UpdateZoomDelayed()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return null;
            UpdateZoom();
        }
    }

    public void EnableCentering(bool enable)
    {
        center = enable;
    }

    void HandleInitialized(bool initialized)
    {
        if (initialized)
        {
            center = true;
            UpdatePositions();
        }
    }

    void ScaleAroundPoint(float scale, Vector2 startPosition)
    {
        Vector3? worldPosition = HandleTouch.GetWorldPosition(startPosition);
        SetCameraSize(targetCamera.orthographicSize * scale);
        if (worldPosition.HasValue)
        {
            // Adjust the camera position so that the world position under the mouse stays the same.
            Vector2 endPosition = targetCamera.WorldToScreenPoint(worldPosition.Value);
            Vector2 offset = startPosition - endPosition;
            targetCamera.transform.position = TargetPositionForOffset(offset.x, offset.y);
        }
    }

    void SetCameraSize(float size)
    {
        targetCamera.orthographicSize = Mathf.Clamp(size, 0.5f, 30f);
    }

    Vector3 TargetPositionForOffset(float x, float y)
    {
        // Camera position so that the board is centered, leaving space on the left for the menu,
        // accounting for the rotation of the camera, and keeping the camera the same distance from the board.
        // TODO: This is making assumptions about the camera's rotation, and I don't like that.
        Vector3 topLeftScreen = targetCamera.WorldToScreenPoint(topLeft);
        Vector3 topRightScreen = targetCamera.WorldToScreenPoint(topRight);
        topLeftScreen.x += x;
        topRightScreen.y += y;
        Vector3 xOffset = topLeft - targetCamera.ScreenToWorldPoint(topLeftScreen);
        Vector3 yOffset = topRight - targetCamera.ScreenToWorldPoint(topRightScreen);
        return targetCamera.transform.position + xOffset + yOffset;
    }
}
