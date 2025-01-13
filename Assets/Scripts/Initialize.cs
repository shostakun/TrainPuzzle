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
        Board.inst.onInitialized += InitializeBoard;
        InitializeBoard(Board.inst.initialized);
    }

    void InitializeBoard(bool initialized)
    {
        if (initialized) return;
        foreach (Initializer initializer in initializers)
        {
            initializer.Initialize();
        }
        Board.inst.initialized = true;
    }

    void OnDestroy()
    {
        Board.inst.onInitialized -= InitializeBoard;
    }
}
