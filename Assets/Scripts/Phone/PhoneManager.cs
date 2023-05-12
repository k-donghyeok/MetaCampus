using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.Rendering.DebugUI;

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

    public void SetHeld(InputDevice device)
    {
        SetHeld(true);
        heldDevice = device;
    }

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
