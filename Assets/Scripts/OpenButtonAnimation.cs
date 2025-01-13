using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class OpenButtonAnimation : MonoBehaviour
{
    public GameObject top;
    public GameObject middle;
    public GameObject bottom;
    public float speed = 25f;
    protected RectTransform middleRect;
    protected RectTransform rectTransform;
    protected float xOffset;
    protected float yOffset;

    void Start()
    {
        InGameMenuManager.inst.onOpenChange += OnOpenChange;
        rectTransform = GetComponent<RectTransform>();
        middleRect = middle.GetComponent<RectTransform>();
        xOffset = middleRect.offsetMin.x;
        yOffset = top.transform.localPosition.y;
    }

    void OnDestroy()
    {
        InGameMenuManager.inst.onOpenChange -= OnOpenChange;
    }

    void OnOpenChange(bool open)
    {
        StartCoroutine(open ? Open() : Close());
    }

    IEnumerator Close()
    {
        while (top.transform.localPosition.y < yOffset - 0.01f)
        {
            top.transform.localPosition = Vector3.Lerp(top.transform.localPosition, Vector3.up * yOffset, Time.deltaTime * speed);
            top.transform.localRotation = Quaternion.Lerp(top.transform.localRotation, Quaternion.identity, Time.deltaTime * speed);
            middleRect.offsetMin = Vector2.Lerp(middleRect.offsetMin, new Vector2(xOffset, middleRect.offsetMin.y), Time.deltaTime * speed);
            middleRect.offsetMax = Vector2.Lerp(middleRect.offsetMax, new Vector2(-xOffset, middleRect.offsetMax.y), Time.deltaTime * speed);
            bottom.transform.localPosition = Vector3.Lerp(bottom.transform.localPosition, Vector3.down * yOffset, Time.deltaTime * speed);
            bottom.transform.localRotation = Quaternion.Lerp(bottom.transform.localRotation, Quaternion.identity, Time.deltaTime * speed);
            yield return null;
        }
        top.transform.localPosition = Vector3.up * yOffset;
        top.transform.localRotation = Quaternion.identity;
        middleRect.offsetMin = new Vector2(xOffset, middleRect.offsetMin.y);
        middleRect.offsetMax = new Vector2(-xOffset, middleRect.offsetMax.y);
        bottom.transform.localPosition = Vector3.down * yOffset;
        bottom.transform.localRotation = Quaternion.identity;
    }

    IEnumerator Open()
    {
        while (top.transform.localPosition.y > 0.1f)
        {
            top.transform.localPosition = Vector3.Lerp(top.transform.localPosition, Vector3.zero, Time.deltaTime * speed);
            top.transform.localRotation = Quaternion.Lerp(top.transform.localRotation, Quaternion.Euler(0, 0, -45), Time.deltaTime * speed);
            middleRect.offsetMin = Vector2.Lerp(middleRect.offsetMin, new Vector2(rectTransform.rect.width * 0.5f, middleRect.offsetMin.y), Time.deltaTime * speed);
            middleRect.offsetMax = Vector2.Lerp(middleRect.offsetMax, new Vector2(rectTransform.rect.width * -0.5f, middleRect.offsetMax.y), Time.deltaTime * speed);
            bottom.transform.localPosition = Vector3.Lerp(bottom.transform.localPosition, Vector3.zero, Time.deltaTime * speed);
            bottom.transform.localRotation = Quaternion.Lerp(bottom.transform.localRotation, Quaternion.Euler(0, 0, 45), Time.deltaTime * speed);
            yield return null;
        }
        top.transform.localPosition = Vector3.zero;
        top.transform.localRotation = Quaternion.Euler(0, 0, -45);
        middleRect.offsetMin = new Vector2(rectTransform.rect.width * 0.5f, middleRect.offsetMin.y);
        middleRect.offsetMax = new Vector2(rectTransform.rect.width * -0.5f, middleRect.offsetMax.y);
        bottom.transform.localPosition = Vector3.zero;
        bottom.transform.localRotation = Quaternion.Euler(0, 0, 45);
    }
}
