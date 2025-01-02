using UnityEngine;

public class DeleteMenuManager : MonoBehaviour
{
    public static DeleteMenuManager inst;

    public Transform container;
    public int itemCount => container.childCount;

    void Awake()
    {
        if (inst == null)
        {
            inst = this;
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ClearItems()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowItems(Deletable[] deletables)
    {
        ClearItems();
        foreach (Deletable deletable in deletables)
        {
            deletable.InstantiateButton(container);
        }
        gameObject.SetActive(true);
    }
}
