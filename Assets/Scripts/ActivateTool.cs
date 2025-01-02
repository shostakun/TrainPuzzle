using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActivateTool : MonoBehaviour
{
    public Image backgroundImage;
    public bool closeSubMenus = false;
    protected bool hasSubMenu => menu != null;
    public Image highlightImage;
    public GameObject iconImageObject;
    public GameObject menu;
    protected MenuSettings settings;
    public GameObject prefab;

    void Start()
    {
        MenuManager.inst.onMenuChange += OnMenuChange;
        ToolManager.inst.onToolChange += OnToolChange;
    }

    void OnEnable()
    {
        settings = GetComponentInParent<MenuSettings>();
        backgroundImage.color = (hasSubMenu || closeSubMenus) ? settings.backgroundColor : settings.subMenuColor;
        highlightImage.color = settings.inactiveColor;
        // OnMenuChange(MenuManager.inst.activeMenu);
        // OnToolChange(ToolManager.inst.currentTool);
    }

    public void Activate()
    {
        if (!hasSubMenu)
        {
            ToolManager.inst.SetTool(this);
            if (closeSubMenus) MenuManager.inst.activeMenu = "";
        }
        else
        {
            MenuManager.inst.activeMenu = menu.name;
        }
    }

    IEnumerator FadeTo(Image image, Color targetColor)
    {
        Color startColor = image.color;
        for (float t = 0; t < 1; t += Time.deltaTime / settings.animationDuration)
        {
            image.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
        image.color = targetColor;
    }

    void OnMenuChange(string menuName)
    {
        if (!hasSubMenu) return;
        if (menu.name == menuName)
        {
            ToolManager.inst.SetTool(this);
            SetColor(backgroundImage, settings.subMenuColor);
        }
        else
        {
            SetColor(backgroundImage, settings.backgroundColor);
        }
    }

    void OnToolChange(string tool)
    {
        if (hasSubMenu) return;
        if (tool == gameObject.name)
        {
            SetColor(highlightImage, settings.activeColor);
        }
        else
        {
            SetColor(highlightImage, settings.inactiveColor);
        }
    }

    void SetColor(Image image, Color color)
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(FadeTo(image, color));
        }
        else
        {
            highlightImage.color = color;
        }
    }
}
