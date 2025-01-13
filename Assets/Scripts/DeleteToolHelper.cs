using UnityEngine;

public class DeleteToolHelper : MonoBehaviour
{
    void Start()
    {
        Track track = GetComponentInParent<Track>();
        if (track == null) return;
        Deletable[] deletables = track.GetComponentsInChildren<Deletable>();
        if (deletables.Length < 1) return;
        DeleteMenuManager.inst.ShowItems(deletables);
        Destroy(gameObject);
        SaveData.inst.Save();
    }
}
