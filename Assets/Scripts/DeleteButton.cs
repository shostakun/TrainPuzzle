using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    public Transform iconContainer;
    public GameObject target;

    public void DeleteTarget()
    {
        if (target != null) Destroy(target);
        if (DeleteMenuManager.inst.itemCount <= 1) DeleteMenuManager.inst.Hide();
        Destroy(gameObject);
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
