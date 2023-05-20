using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotFetcher : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private bool frameRequestPresent = true;
    private byte[] frameByteArray;

    private RenderTexture screenTexture;
    private Texture2D renderedTexture;

    private void Update()
    {
        UpdateFrame();
        if (frameRequestPresent)
        {
            UpdateFrame();
            frameRequestPresent = false;
        }
    }

    public void UpdateFrameRequest()
    {
        frameRequestPresent = true;
    }

    public void UpdateFrame()
    {
        // Create a RenderTexture with the same dimensions as the camera's viewport
        RenderTexture renderTexture = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 24);
        cam.targetTexture = renderTexture;
        cam.Render();

        // Set the active RenderTexture and read the pixels into a Texture2D
        RenderTexture.active = renderTexture;
        Texture2D screenshot = new Texture2D(cam.pixelWidth, cam.pixelHeight);
        screenshot.ReadPixels(new Rect(0, 0, cam.pixelWidth, cam.pixelHeight), 0, 0);

        // Encode the Texture2D as PNG and get the byte array
        frameByteArray = screenshot.EncodeToPNG();

        // Clean up and release resources
        RenderTexture.active = null;
        cam.targetTexture = null;
        Destroy(screenshot);
        Destroy(renderTexture);

    }

    public byte[] GetCameraViewByteArray()
    {
        UpdateFrameRequest();
        return frameByteArray;
    }
}
