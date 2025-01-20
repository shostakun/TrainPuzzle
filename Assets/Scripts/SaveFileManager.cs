using System;
using System.Collections.Generic;
using System.Globalization;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class SaveFileManager : SaveFileBase
{
    public static SaveFileManager inst;

    protected override string filePath => Application.persistentDataPath + "/index.json";

    [ReadOnly]
    public string current = "";
    [ReadOnly]
    public List<string> files = new List<string>();
    public UnityAction<List<string>> onFilesChanged;

    protected static Dictionary<BoardSize, string> boardSizeAffix = new Dictionary<BoardSize, string>
    {
        { BoardSize.Small, "s" },
        { BoardSize.Medium, "m" },
        { BoardSize.Large, "l" }
    };

    void Awake()
    {
        if (inst == null)
        {
            inst = this;
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override void AfterSave()
    {
        isDirty = false;
    }

    protected override void BeforeSave()
    {
        if (current == "") NewBoard(SaveData.inst.boardSize);
        if (files.Count == 0 || files[0] != current)
        {
            files.Remove(current);
            files.Insert(0, current);
            onFilesChanged?.Invoke(files);
        }
    }

    public string GetCurrentFilePath(string ext = "json")
    {
        return GetFilePath(current, ext);
    }

    public string GetFilePath(string name, string ext = "json")
    {
        return $"{Application.persistentDataPath}/{name}.{ext}";
    }

    public void NewBoard(BoardSize size)
    {
        isDirty = true;
        DateTime d = DateTime.UtcNow;
        current = $"b{d.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture)}{boardSizeAffix[size]}";
    }
}
