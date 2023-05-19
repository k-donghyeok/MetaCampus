using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

/// <summary>
/// �ڵ����� �Կ� ����� ����
/// </summary>
public class CaptureBehaviour
{
    private readonly PhoneManager owner;
    private readonly Camera cam;

    private PhotoManager PhotoMgr => owner.PhotoMgr;

    public CaptureBehaviour(PhoneManager owner, Camera cam)
    {
        this.owner = owner;
        this.cam = cam;
    }

    private bool lastTrigger = false;

    public void Update(InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.trigger, out var triggerValue))
        {
            if (triggerValue > 0.9f)
            {
                if (!lastTrigger) PhotoMgr.SaveImage();
                lastTrigger = true;
            }
            else lastTrigger = false;
        }
        else lastTrigger = false;

        //if (Input.mouseScrollDelta.y != 0f)
        //    cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - Input.mouseScrollDelta.y * 2f, 10f, 60f);
    }

    /// <summary>
    /// ī�޶��� ���� ����
    /// </summary>
    /// <param name="adjust">���� Ŭ�����</param>
    public void AdjustZoom(float adjust)
    {
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - adjust, 10f, 60f);
    }
}
