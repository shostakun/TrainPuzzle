using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            if (value) follow = false;
            if (centeringUI) centeringUI.SetActive(interactive && !value);
        }
    }
    public GameObject centeringUI;
    private bool follow_ = false;
    public bool follow
    {
        get => follow_;
        set
        {
            follow_ = value;
            if (value) center = false;
            if (followingUI) followingUI.SetActive(interactive && !value);
        }
    }
    public GameObject followingUI;
    public float followSize = 1.5f;
    public float halfTileSize = 0.5f;
    public bool interactive = true;
    public float menuOffset = 200;
    // public float padding = 10; // TODO: Implement padding.
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
        if (center || follow) UpdateZoom();
    }

    void UpdateGesture()
    {
        if (InputManager.inst.isLocked) return;
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
                    EnableFreeMovement();
                    targetCamera.transform.position = TargetPositionForOffset(delta.x, delta.y);
                }
            }
            return;
        }
        float pinchScale = LeanGesture.GetPinchRatio(fingers);
        if (pinchScale != 1.0f)
        {
            EnableFreeMovement();
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
        if (InputManager.inst.isLocked) return;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            EnableFreeMovement();
            ScaleAroundPoint(1 - scroll, Input.mousePosition);
        }
    }

    void UpdateZoom()
    {
        Rect rect = GetBoardRect();
        float targetSize;
        Vector3 targetPosition;
        Move move;

        if (follow && (move = FindAnyObjectByType<Move>()))
        {
            targetSize = followSize;
            Vector2 center = new Vector2(targetCamera.pixelWidth / 2 + menuOffset / 2,
                targetCamera.pixelHeight / 2);
            Vector2 train = targetCamera.WorldToScreenPoint(move.transform.position);
            Vector2 offset = center - train;
            targetPosition = TargetPositionForOffset(offset.x, offset.y);
        }
        else
        {
            float scaleRatio = Mathf.Max(
                rect.height / targetCamera.pixelHeight,
                rect.width / (targetCamera.pixelWidth - menuOffset));
            targetSize = scaleRatio * targetCamera.orthographicSize;
            targetPosition = TargetPositionForOffset(
                (targetCamera.pixelWidth - rect.xMax - (rect.xMin - menuOffset)) / 2,
                (targetCamera.pixelHeight - rect.yMax - rect.yMin) / 2);
        }

        SetCameraSize(Mathf.Lerp(targetCamera.orthographicSize, targetSize, sensitivity));
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

    public void EnableFollowing(bool enable)
    {
        follow = enable;
    }

    public void EnableFreeMovement()
    {
        center = false;
        follow = false;
    }

    public Rect GetBoardRect()
    {
        Vector3[] points = new Vector3[] { bottomLeft, topRight, topLeft, bottomRight };
        for (int i = 0; i < points.Length; i++)
            points[i] = targetCamera.WorldToScreenPoint(points[i]);
        float top = points.Max(p => p.y);
        float bottom = points.Min(p => p.y);
        float left = points.Min(p => p.x);
        float right = points.Max(p => p.x);
        return new Rect(left, bottom, right - left, top - bottom);
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
        Vector3? worldPosition = CameraUtil.GetWorldPosition(startPosition, targetCamera);
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
