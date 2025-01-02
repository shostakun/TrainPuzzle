using UnityEngine;

public class Deletable : MonoBehaviour
{
    public GameObject deleteButtonPrefab;
    public GameObject iconPrefab;

    public GameObject InstantiateButton(Transform parent)
    {
        GameObject go = Instantiate(deleteButtonPrefab, parent);
        DeleteButton db = go.GetComponent<DeleteButton>();
        db.target = gameObject;
        db.SetIcon(iconPrefab);
        return go;
    }
}
