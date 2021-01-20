// Author: Amay Kataria
// Date: January 18, 2021
// Description: Helper script to take screenshots of a scene. Attach this script to the camera object. Multiple cameras are supported. Every screenshot saved is denoted by the frame number. 
// "Directory Path:" Provide the exact directory where you want the frames to be dropped (NOTE: Watch out for the empty space before the path and after the path). 
// For every run, new folders are created with the current timestamp where all screenshots are saved. For multitple cameras, give two unique directory paths.
// Script will produce an error if it doesn't find a valid directory path.
// "Resolution Factor:" Change to increase the resolution of the image capture. 
// "Update Delta Time:" Most critical number here. This defines the amount of time between each update loop. In every update loop, we create a screenshot.
// A smaller number means a smoother stream of capture and a bigger number means smaller intervals between each capture. Play around with this number to get the right feel for it.
using UnityEngine;
using System.Collections;
using System.IO;
using System; 

public class ScreenRecorder : MonoBehaviour
{
    public string directoryPath = "Null";
    public int resolutionFactor = 1;
    public float updateDeltaTime = 0.033f; 

    private bool _capturing = false; 
    private DirectoryInfo _curDir;
    private Camera _camera;
    private int _resWidth;
    private int _resHeight;
    private Texture2D screenShot;

    void Awake()
    {
        print("Directory: " + directoryPath);

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
        screenShot = new Texture2D(_resWidth, _resHeight, TextureFormat.RGB24, false);
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            _capturing = !_capturing;
            if (_capturing)
            {
                print("Begin Capture.");
                Time.captureDeltaTime = updateDeltaTime;
            } else
            {
                print("Stop Capture.");
                Time.captureDeltaTime = 0.0f;
            }
        }

        if (_capturing)
        {
            capture();
        }
    }

    void capture()
    {
        string filename = _curDir.FullName + "/" + Time.renderedFrameCount + ".png";
        RenderTexture rt = RenderTexture.GetTemporary(_resWidth, _resHeight, 24);
        _camera.targetTexture = rt;
        _camera.Render();
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