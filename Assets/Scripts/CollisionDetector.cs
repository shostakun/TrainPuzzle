using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public enum Priority
    {
        Tree = 10,
        Obstacle = 30,
        Track = 50
    }

    protected HideOnCollision hideOnCollision;
    protected MeshCollider meshCollider;
    public MeshRenderer mesh;
    protected Rigidbody meshRigidbody;
    public Priority priority = Priority.Tree;

    protected void EnsureMesh()
    {
        if (mesh == null) mesh = GetComponentInChildren<MeshRenderer>();
    }

    public static void Install(GameObject go)
    {
        foreach (CollisionDetector detector in go.GetComponentsInChildren<CollisionDetector>())
        {
            detector.Install();
        }
    }

    public void Install()
    {
        // Make sure we have a mesh.
        EnsureMesh();
        if (mesh == null) return;
        // Make sure we have a collider.
        MeshCollider mc = mesh.GetComponent<MeshCollider>();
        if (mc == null)
        {
            mc = mesh.gameObject.AddComponent<MeshCollider>();
            meshCollider = mc;
        }
        mc.sharedMesh = mesh.GetComponent<MeshFilter>().sharedMesh;
        mc.convex = true;
        mc.isTrigger = true;
        // Make sure we have a rigidbody.
        Rigidbody rb = mesh.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = mesh.gameObject.AddComponent<Rigidbody>();
            meshRigidbody = rb;
        }
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        // Make sure we have a HideOnCollision script.
        HideOnCollision hoc = mesh.GetComponent<HideOnCollision>();
        if (hoc == null)
        {
            hoc = mesh.gameObject.AddComponent<HideOnCollision>();
            hideOnCollision = hoc;
        }
        hoc.mesh = mesh;
        hoc.priority = priority;
    }

    public static void Uninstall(GameObject go, bool preserveHidden = false)
    {
        foreach (CollisionDetector detector in go.GetComponentsInChildren<CollisionDetector>())
        {
            detector.Uninstall(preserveHidden);
        }
    }

    public void Uninstall(bool preserveHidden = false)
    {
        if (meshCollider != null) Destroy(meshCollider);
        if (meshRigidbody != null) Destroy(meshRigidbody);
        if (hideOnCollision != null)
        {
            if (mesh != null) mesh.enabled = !(preserveHidden && hideOnCollision.shouldHide);
            Destroy(hideOnCollision);
        }
    }
}
