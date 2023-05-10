using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CaptureManager : MonoBehaviour
{
    public static int ImageIndex { get; private set; } = 0;

    private RenderTexture RT;
    private Camera cam;
    public RawImage rawImage;

    private void Awake()
    {
        RT = Resources.Load("Textures/CaptureRenderTexture") as RenderTexture;
        cam = GetComponent<Camera>();
    }

    private void SaveImage()
    {
        Texture2D texture = new(RT.width, RT.height, TextureFormat.ARGB32, false);
        RenderTexture.active = RT;
        texture.ReadPixels(new(0f, 0f, RT.width, RT.height), 0, 0);
        texture.Apply();

        var bytes = texture.EncodeToPNG();
        File.WriteAllBytes(GetPath(ImageIndex), bytes);
        ++ImageIndex;
    }

    private void LoadImage(int index)
    {
        Texture2D texture = new(RT.width, RT.height, TextureFormat.ARGB32, false);
        var bytes = File.ReadAllBytes(GetPath(index));

        texture.LoadImage(bytes);
        texture.Apply();
        rawImage.texture = texture;
    }

    private static string GetPath(int index)
        => $"{Application.persistentDataPath}/Capture{index}.png";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) SaveImage();
        
        if (Input.mouseScrollDelta.y != 0f)
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - Input.mouseScrollDelta.y * 2f, 10f, 60f);
    }
}
