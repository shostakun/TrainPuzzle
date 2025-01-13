using UnityEngine;

public abstract class Initializer: MonoBehaviour
{
    public abstract void Initialize();
}

public class Initialize : MonoBehaviour
{
    public Initializer[] initializers;

    public void DoInitialization()
    {
        if (Board.inst.initialized) return;
        foreach (Initializer initializer in initializers)
        {
            initializer.Initialize();
        }
        Board.inst.initialized = true;
    }
}
