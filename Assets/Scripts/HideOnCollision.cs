using UnityEngine;

public class HideOnCollision : MonoBehaviour
{
    public MeshRenderer mesh;
    public CollisionDetector.Priority priority = 0;
    public bool shouldHide = false;

    void OnTriggerEnter(Collider collider)
    {
        HideOnCollision other = collider.GetComponent<HideOnCollision>();
        if (other != null && !other.shouldHide && other.priority > priority)
        {
            shouldHide = true;
            if (mesh != null) mesh.enabled = false;
        }
    }
}
