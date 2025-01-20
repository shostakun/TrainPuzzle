using System.Collections.Generic;
// using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopulateFileList : MonoBehaviour
{
    public GameObject fileListContainer;
    public GameObject fileTilePrefab;
    public float minCellSize = 200;

    void Awake()
    {
        gameObject.SetActive(false);
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
        if (SaveFileManager.inst?.onFilesChanged != null)
            SaveFileManager.inst.onFilesChanged -= Populate;
        InputManager.inst.Unlock("PopulateFileList");
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
            Button btn = fileTile.GetComponentInChildren<Button>();
            if (btn != null)
                btn.onClick.AddListener(() =>
                {
                    SaveFileManager.inst.current = file;
                    SaveData.inst.Load();
                    gameObject.SetActive(false);
                });
            // TextMeshProUGUI tmp = fileTile.GetComponentInChildren<TextMeshProUGUI>();
            // if (tmp != null) tmp.text = file;
            Image img = fileTile.GetComponentInChildren<Image>();
            if (img != null)
                img.sprite = IMG2Sprite.LoadNewSprite(
                    SaveFileManager.inst.GetFilePath(file, "png"));
        }
    }
}
