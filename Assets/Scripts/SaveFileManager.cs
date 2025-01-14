using System;
using System.Collections.Generic;
using System.Globalization;
using NaughtyAttributes;
using UnityEngine;

public class SaveFileManager : SaveFileBase
{
    public static SaveFileManager inst;

    protected override string filePath => Application.persistentDataPath + "/index.json";
    public string dataFilePath => $"{Application.persistentDataPath}/{current}.json";

    [ReadOnly]
    public string current = "";
    [ReadOnly]
    public List<string> files = new List<string>();

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
        if (!files.Contains(current)) files.Add(current);
    }

    public void NewBoard(BoardSize size)
    {
        isDirty = true;
        DateTime d = DateTime.UtcNow;
        current = $"b{d.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture)}{boardSizeAffix[size]}";
    }
}
