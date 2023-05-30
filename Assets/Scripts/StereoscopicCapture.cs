using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StereoscopicCapture : MonoBehaviour
{
    public Camera mainCamera;
    public RenderTexture cubeMapRenderTextureLeft;
    public RenderTexture cubeMapRenderTextureRight;
    public RenderTexture equirectRenderTexture;

    void Start()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Capture();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Capture();
        }
    }

    public void Capture()
    {
        mainCamera.stereoSeparation = 0.065f;
        mainCamera.RenderToCubemap(cubeMapRenderTextureLeft, 63, Camera.MonoOrStereoscopicEye.Left);
        mainCamera.RenderToCubemap(cubeMapRenderTextureRight, 63, Camera.MonoOrStereoscopicEye.Right);
        cubeMapRenderTextureLeft.ConvertToEquirect(equirectRenderTexture, Camera.MonoOrStereoscopicEye.Left);
        cubeMapRenderTextureRight.ConvertToEquirect(equirectRenderTexture, Camera.MonoOrStereoscopicEye.Right);

        Texture2D tex = new Texture2D(equirectRenderTexture.width, equirectRenderTexture.height);
        RenderTexture.active = equirectRenderTexture;
        tex.ReadPixels(new Rect(0, 0, equirectRenderTexture.width, equirectRenderTexture.height), 0, 0);
        RenderTexture.active = null;
        byte[] bytes = tex.EncodeToJPG();
        string path = Application.dataPath + "/StereoPanorama.jpg";
        System.IO.File.WriteAllBytes(path, bytes);
    }
}