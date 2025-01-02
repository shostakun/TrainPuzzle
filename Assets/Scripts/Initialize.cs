using UnityEngine;

public abstract class Initializer: MonoBehaviour
{
    public abstract void Initialize();
}

public class Initialize : MonoBehaviour
{
    public Initializer[] initializers;

    void Start()
    {
        foreach (Initializer initializer in initializers)
        {
            initializer.Initialize();
        }
    }
}
