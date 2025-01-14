using System.IO;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public Camera targetCamera;

    void Start()
    {
        if (targetCamera == null) targetCamera = Camera.main;
    }

    public void Take(string filePath)
    {
        // Capture the screen.
        RenderTexture rt = new RenderTexture(targetCamera.pixelWidth, targetCamera.pixelHeight, 24);
        targetCamera.targetTexture = rt;
        targetCamera.Render();
        RenderTexture.active = rt;

        // Create a Texture2D to hold the captured image.
        Texture2D screenShot = new Texture2D(targetCamera.pixelWidth, targetCamera.pixelHeight);
        screenShot.ReadPixels(new Rect(0, 0, targetCamera.pixelWidth, targetCamera.pixelHeight), 0, 0);
        targetCamera.targetTexture = null;
        RenderTexture.active = null; // Clean up.

        // Save a PNG to the persistent data path.
        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);

        // Destroy the temporary textures.
        Destroy(screenShot);
        Destroy(rt);
    }
}
