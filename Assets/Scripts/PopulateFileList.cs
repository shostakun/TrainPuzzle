using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateFileList : MonoBehaviour
{
    public GameObject deleteConfirmation;
    public GameObject fileListContainer;
    public GameObject fileTilePrefab;
    public float minCellSize = 200;

    void Awake()
    {
        gameObject.SetActive(false);
        if (deleteConfirmation != null) deleteConfirmation.SetActive(false);
    }

    void Update()
    {
        RectTransform rt = fileListContainer.GetComponent<RectTransform>();
        GridLayoutGroup glg = fileListContainer.GetComponent<GridLayoutGroup>();
        if (rt != null && glg != null)
        {
            float aspectRatio = glg.cellSize.y / glg.cellSize.x;
            int cols = Mathf.FloorToInt(rt.rect.width / minCellSize);
            float spacing = glg.spacing.x * cols; // Extra spacing on the right for the scrollbar.
            int width = Mathf.FloorToInt((rt.rect.width - spacing) / cols);
            glg.cellSize = new Vector2(width, width * aspectRatio);
        }
    }

    void OnEnable()
    {
        InputManager.inst.Lock("PopulateFileList");
        SaveFileManager.inst.onFilesChanged += Populate;
        Populate(SaveFileManager.inst.files);
    }

    void OnDisable()
    {
        if (SaveFileManager.inst != null && SaveFileManager.inst.onFilesChanged != null)
            SaveFileManager.inst.onFilesChanged -= Populate;
        if (InputManager.inst != null)
            InputManager.inst.Unlock("PopulateFileList");
    }

    public void ConfirmDelete(string file)
    {
        PopulateFileTile pft = deleteConfirmation.GetComponentInChildren<PopulateFileTile>();
        if (pft != null) pft.Populate(file, this);
        deleteConfirmation.SetActive(true);
    }

    void Populate(List<string> files)
    {
        foreach (Transform child in fileListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (string file in files)
        {
            GameObject fileTile = Instantiate(fileTilePrefab, fileListContainer.transform);
            PopulateFileTile pft = fileTile.GetComponent<PopulateFileTile>();
            if (pft != null) pft.Populate(file, this);
        }
    }
}
