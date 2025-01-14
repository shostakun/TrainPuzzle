using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class SaveFileBase : MonoBehaviour
{
    protected bool isDirty = true;
    protected virtual string filePath => Application.persistentDataPath + "/data.json";

    protected virtual void AfterLoad()
    {

    }

    protected virtual void AfterSave()
    {

    }

    protected virtual void BeforeSave()
    {

    }

    protected void Load()
    {
        try
        {
            if (File.Exists(filePath))
            {
                using StreamReader sr = new StreamReader(filePath);
                JsonUtility.FromJsonOverwrite(sr.ReadToEnd(), this);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        AfterLoad();
    }

    public void Save()
    {
        BeforeSave();
        if (!isDirty) return;

        try
        {
            using StreamWriter sw = new StreamWriter(filePath);
            sw.Write(JsonUtility.ToJson(this));
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        AfterSave();
    }

    public void SaveNextFrame()
    {
        StartCoroutine(SaveNextFrameCoroutine());
    }

    protected IEnumerator SaveNextFrameCoroutine()
    {
        yield return null;
        Save();
    }
}
