using UnityEngine;
using UnityEngine.Events;

public class InGameMenuManager : MonoBehaviour
{
    public static InGameMenuManager inst;

    public GameObject menuContent;
    private bool _isOpen = false;
    public bool isOpen
    {
        get
        {
            return _isOpen;
        }
        protected set
        {
            bool changed = _isOpen != value;
            _isOpen = value;
            if (changed) onOpenChange?.Invoke(value);
        }
    }
    public UnityAction<bool> onOpenChange;

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
        menuContent.SetActive(isOpen);
    }

    public void Toggle()
    {
        isOpen = !isOpen;
        menuContent.SetActive(isOpen);
    }
}
