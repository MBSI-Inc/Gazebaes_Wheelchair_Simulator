using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotFetcher : MonoBehaviour
{
    [SerializeField]
    Camera cam;

    bool frameRequestPresent = true;
    byte[] frameByteArray;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveCameraView(cam);
        }
        if (frameRequestPresent)
        {
            UpdateFrame();
            frameRequestPresent = false;
        }
    }
    void SaveCameraView(Camera cam)
    {
        
        //System.IO.File.WriteAllBytes("/Users/joshuachung/Coding/MBSI/Temp" + "/cameracapture.png", byteArray);
        //UDPSenderTester.SendScreenShot(byteArray);

    }

    public void UpdateFrameRequest()
    {
        frameRequestPresent = true;
    }
    public void UpdateFrame()
    {
        new WaitForEndOfFrame();
        RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 16);
        cam.targetTexture = screenTexture;
        RenderTexture.active = screenTexture;
        cam.Render();
        Texture2D renderedTexture = new Texture2D(Screen.width, Screen.height);
        renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        RenderTexture.active = null;
        frameByteArray = renderedTexture.EncodeToPNG();
    }

    public byte[] GetCameraViewByteArray()
    {
        UpdateFrameRequest();
        return frameByteArray;
    }

}

