using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MenuSettings))]
public class MenuManager : MonoBehaviour
{
    public static MenuManager inst { get; private set; }

    private string _activeMenu = "";
    public string activeMenu
    {
        get
        {
            return _activeMenu;
        }
        set
        {
            bool changed = _activeMenu != value;
            _activeMenu = value;
            Debug.Log("Active menu: " + value);
            ShowActiveMenu();
            if (changed) onMenuChange?.Invoke(value);
        }
    }

    public Transform[] menuContainers;
    public UnityAction<string> onMenuChange;
    private MenuSettings settings;

    void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        settings = GetComponent<MenuSettings>();
        ShowActiveMenu();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && TrackToolManager.inst.currentTool == "" && activeMenu != "")
        {
            activeMenu = "";
        }
    }

    IEnumerator FadeTo(GameObject menu, float targetAlpha)
    {
        CanvasGroup canvasGroup = menu.GetComponentInChildren<CanvasGroup>();
        canvasGroup.blocksRaycasts = targetAlpha > 0;
        canvasGroup.interactable = targetAlpha > 0;
        for (float t = 0; t < 1; t += Time.deltaTime / settings.animationDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, t);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }

    public void ShowActiveMenu()
    {
        foreach (Transform container in menuContainers)
        {
            foreach (Transform child in container)
            {
                StartCoroutine(FadeTo(child.gameObject, child.name == activeMenu ? 1 : 0));
            }
        }
    }
}
