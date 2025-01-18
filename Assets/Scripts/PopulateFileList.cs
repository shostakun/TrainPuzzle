using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopulateFileList : MonoBehaviour
{
    public GameObject fileListContainer;
    public GameObject fileTilePrefab;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        SaveFileManager.inst.onFilesChanged += Populate;
        Populate(SaveFileManager.inst.files);
    }

    void OnDisable()
    {
        if (SaveFileManager.inst?.onFilesChanged != null)
            SaveFileManager.inst.onFilesChanged -= Populate;
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
