using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    public Transform iconContainer;
    public GameObject target;

    public void DeleteTarget()
    {
        if (target != null) Destroy(target);
        DestroyImmediate(gameObject);
        if (DeleteMenuManager.inst.itemCount < 1) DeleteMenuManager.inst.Hide();
    }

    public void SetIcon(GameObject iconPrefab)
    {
        if (iconPrefab == null) return;
        foreach (Transform child in iconContainer)
        {
            Destroy(child.gameObject);
        }
        Instantiate(iconPrefab, iconContainer);
    }
}
