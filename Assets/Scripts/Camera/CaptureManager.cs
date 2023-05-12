using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �ڵ����� �Կ� ����� ����
/// </summary>
[RequireComponent(typeof(Camera))]
public class CaptureManager : MonoBehaviour
{
    /// <summary>
    /// ������ �Կ��� ���� �ε�����
    /// </summary>
    public static int ImageIndex { get; private set; } = 0;

    private RenderTexture RT;
    private Camera cam;

    private void Awake()
    {
        RT = Resources.Load("Textures/CaptureRenderTexture") as RenderTexture;
        cam = GetComponent<Camera>();
    }

    /// <summary>
    /// ������ �Կ��ϰ� ����
    /// </summary>
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

    /// <summary>
    /// ����� �̹����� �ҷ���
    /// </summary>
    /// <param name="index">�ҷ��� �̹��� �ε���</param>
    /// <returns>�ҷ��� �̹��� �ؽ���</returns>
    /// <exception cref="FileNotFoundException">�������� �ʴ� �ε����� ���</exception>
    private Texture2D LoadImage(int index)
    {
        Texture2D texture = new(RT.width, RT.height, TextureFormat.ARGB32, false);
        string path = GetPath(index);
        if (!File.Exists(path)) throw new FileNotFoundException("�� �ε����� �̹����� �������� �ʽ��ϴ�.");
        var bytes = File.ReadAllBytes(path);

        texture.LoadImage(bytes);
        texture.Apply();
        return texture;
    }

    /// <summary>
    /// ����� �̹����� ���� ��ġ�� ����
    /// </summary>
    /// <param name="index">���ϴ� �̹��� �ε���</param>
    /// <returns>���� ���</returns>
    private static string GetPath(int index)
        => $"{Application.persistentDataPath}/Capture{index}.png";

    
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) SaveImage();
        
        if (Input.mouseScrollDelta.y != 0f)
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - Input.mouseScrollDelta.y * 2f, 10f, 60f);
    }
    */

    /// <summary>
    /// ī�޶��� ���� ����
    /// </summary>
    /// <param name="adjust">���� Ŭ�����</param>
    public void AdjustZoom(float adjust)
    {
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - adjust, 10f, 60f);
    }

}
