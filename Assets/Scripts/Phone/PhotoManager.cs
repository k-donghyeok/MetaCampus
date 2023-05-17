using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PhotoManager
{
    /// <summary>
    /// 다음에 촬영할 사진 인덱스값
    /// </summary>
    public int ImageIndex { get; private set; } = 0;

    private readonly RenderTexture RT;

    public PhotoManager(int imageIndex)
    {
        ImageIndex = imageIndex;
        RT = Resources.Load("Textures/CaptureRenderTexture") as RenderTexture;
    }


    /// <summary>
    /// 사진을 촬영하고 저장
    /// </summary>
    public void SaveImage()
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
    /// 저장된 이미지를 불러옴
    /// </summary>
    /// <param name="index">불러올 이미지 인덱스</param>
    /// <returns>불러온 이미지 텍스쳐</returns>
    /// <exception cref="FileNotFoundException">존재하지 않는 인덱스를 사용</exception>
    public Texture2D LoadImage(int index)
    {
        Texture2D texture = new(RT.width, RT.height, TextureFormat.ARGB32, false);
        string path = GetPath(index);
        if (!File.Exists(path)) throw new FileNotFoundException("이 인덱스의 이미지는 존재하지 않습니다.");
        var bytes = File.ReadAllBytes(path);

        texture.LoadImage(bytes);
        texture.Apply();
        return texture;
    }

    /// <summary>
    /// 저장된 이미지의 파일 위치를 생성
    /// </summary>
    /// <param name="index">원하는 이미지 인덱스</param>
    /// <returns>파일 경로</returns>
    private static string GetPath(int index)
        => $"{Application.persistentDataPath}/Capture{index}.png";




}