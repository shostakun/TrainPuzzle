using UnityEngine;

public class RandomInstance : MonoBehaviour, ToolRefresher
{
    public Vector3 offset;
    public GameObject[] prefabs;
    public float scale = 0.7f;

    void Start()
    {
        if (transform.childCount < 1) Refresh();
    }

    public void Refresh()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        int index = Random.Range(0, prefabs.Length);
        GameObject go = Instantiate(prefabs[index], transform);
        go.name = prefabs[index].name;
        go.transform.position += offset;
        go.transform.localScale = Vector3.one * scale;
        go.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }
}
