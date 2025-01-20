using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public static InputManager inst;

    protected List<string> lockers_ = new List<string>();
    public bool isLocked => lockers_.Count > 0;
    public UnityAction<bool> onLocked;

    void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Lock(string locker)
    {
        if (!lockers_.Contains(locker))
        {
            lockers_.Add(locker);
            if (lockers_.Count == 1) onLocked?.Invoke(isLocked);
        }
    }

    public void Unlock(string locker)
    {
        if (lockers_.Contains(locker))
        {
            lockers_.Remove(locker);
            if (lockers_.Count == 0) onLocked?.Invoke(isLocked);
        }
    }
}
