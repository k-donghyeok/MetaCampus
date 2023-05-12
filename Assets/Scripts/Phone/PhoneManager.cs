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
    /// �̰��� �÷��̾� �տ� ����ְ�, ��ȣ�ۿ��� �������� ����
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

    private void Update()
    {
        if (!Held)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0f) gameObject.SetActive(false);
            return;
        }


    }

}
