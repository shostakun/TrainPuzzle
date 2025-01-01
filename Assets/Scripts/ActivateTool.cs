using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActivateTool : MonoBehaviour
{
    public Image backgroundImage;
    public Image highlightImage;
    public GameObject menu;
    protected MenuSettings settings;
    public GameObject trackPrefab;

    void Start()
    {
        MenuManager.inst.onMenuChange += OnMenuChange;
        TrackToolManager.inst.onToolChange += OnToolChange;
    }

    void OnEnable()
    {
        settings = GetComponentInParent<MenuSettings>();
        backgroundImage.color = settings.backgroundColor;
        highlightImage.color = settings.inactiveColor;
        // OnMenuChange(MenuManager.inst.activeMenu);
        // OnToolChange(TrackToolManager.inst.currentTool);
    }

    public void Activate()
    {
        if (menu == null)
        {
            TrackToolManager.inst.SetTool(this);
        }
        else
        {
            MenuManager.inst.activeMenu = menu.name;
        }
    }

    IEnumerator FadeTo(Color targetColor)
    {
        for (float t = 0; t < 1; t += Time.deltaTime / settings.animationDuration)
        {
            highlightImage.color = Color.Lerp(settings.inactiveColor, targetColor, t);
            yield return null;
        }
        highlightImage.color = targetColor;
    }

    void OnMenuChange(string menuName)
    {
        if (menu == null) return;
        if (menu.name == menuName)
        {
            TrackToolManager.inst.SetTool(this);
            SetColor(settings.activeColor);
        }
        else
        {
            SetColor(settings.inactiveColor);
        }
    }

    void OnToolChange(string tool)
    {
        if (menu != null) return;
        if (tool == gameObject.name)
        {
            SetColor(settings.activeColor);
        }
        else
        {
            SetColor(settings.inactiveColor);
        }
    }

    void SetColor(Color color)
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(FadeTo(color));
        }
        else
        {
            highlightImage.color = color;
        }
    }
}
