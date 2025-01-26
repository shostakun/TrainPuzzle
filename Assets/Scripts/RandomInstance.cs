using UnityEngine;

public class RandomInstance : MonoBehaviour, ToolRefresher
{
    public Vector3 offset;
    public GameObject[] prefabs;
    public float scale = 0.7f;

    void Awake()
    {
        // Needs to be in Awake so the instance is created immediately on instantiation.
        if (transform.childCount < 1) Refresh();
    }

    public void Refresh()
    {
        RefreshWithPrefab(
            prefabs[Random.Range(0, prefabs.Length)],
            Quaternion.Euler(0, Random.Range(0, 360), 0));
    }

    public void RefreshWithPrefab(GameObject prefab, Quaternion rotation)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        GameObject go = Instantiate(prefab, transform);
        go.name = prefab.name;
        go.transform.position += offset;
        go.transform.localScale = Vector3.one * scale;
        go.transform.rotation = rotation;
    }
}
