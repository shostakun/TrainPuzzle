using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeleteLockRule : MonoBehaviour, ToolLockRule
{
    public bool IsLocked(Track track)
    {
        if (ToolManager.inst.currentObj != null)
        {
            Deletable[] deletables = track.GetComponentsInChildren<Deletable>();
            if (deletables.Length == 0) return true;
        }
        return false;
    }

    public static string RemoveDups(string str)
    {
        string result = "";
        foreach (char c in str)
        {
            if (result.IndexOf(c) < 0)
            {
                result += c;
            }
        }
        return result;
    }
}
