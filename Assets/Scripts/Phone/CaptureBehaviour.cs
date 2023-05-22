using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

/// <summary>
/// 핸드폰의 촬영 기능을 관리
/// </summary>
public class CaptureBehaviour
{
    private readonly PhoneManager owner;
    private readonly Camera cam;
    private readonly RenderTexture RT;

    public CaptureBehaviour(PhoneManager owner, Camera cam)
    {
        this.owner = owner;
        this.cam = cam;
        RT = Resources.Load("Textures/CaptureRenderTexture") as RenderTexture;
    }

    private bool lastTrigger = false;

    public void Update(InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.trigger, out var triggerValue))
        {
            if (triggerValue > 0.9f)
            {
                if (!lastTrigger)
                {
                    SaveImage();
                    owner.UpdatePhoto(photo);
                    owner.ChangeMode(PhoneManager.Mode.Attach);
                }
                lastTrigger = true;
            }
            else lastTrigger = false;
        }
        else lastTrigger = false;

        //if (Input.mouseScrollDelta.y != 0f)
        //    cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - Input.mouseScrollDelta.y * 2f, 10f, 60f);
    }

    private Texture2D photo = null;

    /// <summary>
    /// 사진을 촬영하고 저장
    /// </summary>
    public void SaveImage()
    {
        photo = new(RT.width, RT.height, TextureFormat.ARGB32, false);
        RenderTexture.active = RT;
        photo.ReadPixels(new(0f, 0f, RT.width, RT.height), 0, 0);
        photo.Apply();
    }

    /// <summary>
    /// 카메라의 줌을 조절
    /// </summary>
    /// <param name="adjust">양이 클로즈업</param>
    public void AdjustZoom(float adjust)
    {
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - adjust, 10f, 60f);
    }
}
