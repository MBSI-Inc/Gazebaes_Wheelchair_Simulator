using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotFetcher : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private byte[] frameByteArray;
    private Texture2D renderedTexture;
    private RenderTexture screenTexture;

    //// Update is called once per frame
    //private void Update()
    //{
    //    if (frameRequestPresent)
    //    {
    //        UpdateFrame();
    //        frameRequestPresent = false;
    //    }
    //}

    //public void UpdateFrameRequest()
    //{
    //    frameRequestPresent = true;
    //}

    //public void UpdateFrame()
    //{
    //    new WaitForEndOfFrame();
    //    screenTexture = new RenderTexture(Screen.width, Screen.height, 16);
    //    cam.targetTexture = screenTexture;
    //    RenderTexture.active = screenTexture;
    //    cam.Render();
    //    renderedTexture = new Texture2D(Screen.width, Screen.height);
    //    renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
    //    RenderTexture.active = null;
    //    frameByteArray = renderedTexture.EncodeToPNG();
    //}

    public IEnumerator UpdateFrameRequest()
    {
        yield return new WaitForEndOfFrame();
        screenTexture = new RenderTexture(Screen.width, Screen.height, 16);
        cam.targetTexture = screenTexture;
        RenderTexture.active = screenTexture;
        cam.Render();
        renderedTexture = new Texture2D(Screen.width, Screen.height);
        renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        RenderTexture.active = null;
        frameByteArray = renderedTexture.EncodeToPNG();
        yield return null;
    }

    public byte[] GetCameraViewByteArray()
    {
        UpdateFrameRequest();
        return frameByteArray;
    }
}
