using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redirect_Texture : MonoBehaviour
{
    // Camera to get the texture from. 
    public Camera myCamera;
    public int resolutionFactor = 1;

    private Renderer _renderer;
    private int _resWidth;
    private int _resHeight;
    private Texture2D _screenShot; 

    // Start is called before the first frame update
    void Start()
    {
        _renderer = gameObject.GetComponent<Renderer>();
        _resWidth = myCamera.pixelWidth * resolutionFactor;
        _resHeight = myCamera.pixelHeight * resolutionFactor;
        _screenShot = new Texture2D(_resWidth, _resHeight, TextureFormat.RGB24, false);
        _renderer.material.SetFloat("_Metallic", 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        // Record a shot. 
        capture();

        // Set the texture.
        _renderer.material.mainTexture = _screenShot; 
    }

    void capture()
    {
        RenderTexture rt = RenderTexture.GetTemporary(_resWidth, _resHeight, 24);
        myCamera.targetTexture = rt;
        myCamera.Render();
        RenderTexture.active = rt;

        _screenShot.ReadPixels(myCamera.pixelRect, 0, 0);
        _screenShot.Apply();

        //byte[] bytes = screenShot.EncodeToPNG();
        //File.WriteAllBytes(filename, bytes);

        myCamera.targetTexture = null;
        RenderTexture.active = null;
        rt.Release();
    }
}
