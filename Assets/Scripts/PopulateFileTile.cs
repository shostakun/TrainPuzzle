using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopulateFileTile : MonoBehaviour
{
    public bool allowDelete = true;
    public Button deleteButton;
    protected string file;
    public Image image;
    public Button loadButton;
    public TextMeshProUGUI loadButtonText;
    protected PopulateFileList parent;

    public void DeleteFile()
    {
        SaveFileManager.inst.DeleteFile(file);
    }

    protected void HandleConfirmDelete()
    {
        if (parent != null) parent.ConfirmDelete(file);
    }

    protected void HandleLoad()
    {
        SaveFileManager.inst.SetCurrent(file);
        SaveData.inst.Load();
        if (parent != null) parent.gameObject.SetActive(false);
    }

    public void Populate(string file, PopulateFileList parent)
    {
        this.file = file;
        this.parent = parent;

        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(HandleConfirmDelete);
            deleteButton.gameObject.SetActive(
                allowDelete && file != SaveFileManager.inst.current);
        }

        if (loadButton != null)
            loadButton.onClick.AddListener(HandleLoad);

        Sprite sprite;
        if (image != null && (
            sprite = IMG2Sprite.LoadNewSprite(
                SaveFileManager.inst.GetFilePath(file, "png"))
            ) != null)
        {
            image.sprite = sprite;
        }
        else if (loadButtonText != null) loadButtonText.text = file;
    }
}
