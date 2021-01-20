// Author: Amay Kataria
// Date: January 18, 2021
// Description: Helper script to capture on the camera.
// Provide a path where you want the frames to be dropped.
// Provide a folder name in which the frames should be created. If it's null, it'll use current system time to
// create a folder. 

using UnityEngine;
using System.Collections;
using System.IO;
using System; 

public class ScreenRecorder : MonoBehaviour
{
    public string directoryPath = "Null";
    public int resolutionFactor = 1; 

    //amount of frames you want to record before closing the game
    private bool active = false; 
    private int _frameNum = 0;
    private DirectoryInfo _curDir;
    private Camera _camera;
    private int _resWidth;
    private int _resHeight; 

    void Awake()
    {
        // Check if we have received a valid path. 
        if (!Directory.Exists(directoryPath))
        {
            Debug.LogError("Error: Please provide a directory location.");
            return; 
        }

        // Create a new directory based on the current timestamp in the directory provided. 
        long curTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        string path = directoryPath + "/" + curTimestamp.ToString();
        _curDir = Directory.CreateDirectory(path);

        // Store camera.
        _camera = GetComponent<Camera>();
        _resWidth = _camera.pixelWidth * resolutionFactor;
        _resHeight = _camera.pixelHeight * resolutionFactor; 
    }

    void Update()
    {
        //if (Keyboard.current.pKey.wasPressedThisFrame)
        //{
        //    active = !active;
        //    if (active) Time.captureFramerate = 60;
        //}

        //if (!active)
        //    return;
        capture();

        // Next frame. 
        _frameNum++; 
    }

    void capture()
    {
        string filename = _curDir.FullName + "/" + _frameNum + ".png";
        RenderTexture rt = new RenderTexture(_resWidth, _resHeight, 24);
        _camera.targetTexture = rt;
        _camera.Render();
        Texture2D screenShot = new Texture2D(_resWidth, _resHeight, TextureFormat.RGB24, false);
        RenderTexture.active = rt;
        screenShot.ReadPixels(_camera.pixelRect, 0, 0);
        screenShot.Apply();
        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(filename, bytes);
        _camera.targetTexture = null;
        RenderTexture.active = null;
        rt.Release();
    }
}


//ScreenCapture.CaptureScreenshot(fileName, resolutionFactor);