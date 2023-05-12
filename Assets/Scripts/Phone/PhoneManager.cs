using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// 핸드폰의 전반적인 행동 관리
/// </summary>
public class PhoneManager : MonoBehaviour
{
    private CaptureManager captureManager = null;

    private void Awake()
    {
        captureManager = GetComponentInChildren<CaptureManager>();
    }

    /// <summary>
    /// 이것이 플레이어 손에 들려있고, 상호작용이 가능한지 여부
    /// </summary>
    public bool Held { get; private set; }

    private float hideTimer = 1f;

    private InputDevice heldDevice;

    /// <summary>
    /// 핸드폰을 잡은 컨트롤러 설정
    /// </summary>
    /// <param name="device"></param>
    public void SetHeld(InputDevice device)
    {
        SetHeld(true);
        heldDevice = device;
    }

    /// <summary>
    /// 핸드폰을 놓기
    /// </summary>
    /// <param name="held">핸드폰을 놓음 여부</param>
    public void SetHeld(bool held = false)
    {
        if (Held == held) return;
        Held = held;
        hideTimer = 1f;
    }

    private bool lastTrigger = false;

    private void Update()
    {
        if (!Held)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0f) gameObject.SetActive(false);
            return;
        }
        if (!heldDevice.isValid) return;

        if (heldDevice.TryGetFeatureValue(CommonUsages.trigger, out var triggerValue))
        {
            if (triggerValue > 0.9f)
            {
                if (!lastTrigger) captureManager.SaveImage();
                lastTrigger = true;
            }
            else lastTrigger = false;
        }
        else lastTrigger = false;

    }

}
