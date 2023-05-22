using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// 사진기의 전반적인 행동 관리
/// </summary>
public class PhoneManager : MonoBehaviour
{
    [SerializeField]
    private Camera cam = null;

    [SerializeField]
    private MeshRenderer screen = null;

    [SerializeField]
    private Canvas canvas = null;

    [SerializeField]
    private HandMapManager map = null;

    public Canvas Canvas() => canvas;

    public CaptureBehaviour CaptureBehav { get; private set; } = null;
    public AttachBehaviour AttachBehav { get; private set; } = null;

    public Texture2D Photo { get; private set; } = null;

    public void UpdatePhoto(Texture2D newPhoto) => Photo = newPhoto;

    private void Awake()
    {
        CaptureBehav = new CaptureBehaviour(this, cam);
        AttachBehav = new AttachBehaviour(this);
    }

    /// <summary>
    /// 이것이 플레이어 손에 들려있고, 상호작용이 가능한지 여부
    /// </summary>
    public bool Held { get; private set; }

    private float hideTimer = 1f;

    private InputDevice heldDevice;

    /// <summary>
    /// 사진기를 잡은 컨트롤러 설정
    /// </summary>
    /// <param name="device"></param>
    public void SetHeld(InputDevice device)
    {
        SetHeld(true);
        heldDevice = device;
    }

    /// <summary>
    /// 사진기를 놓기
    /// </summary>
    /// <param name="held">사진기를 놓음 여부</param>
    public void SetHeld(bool held = false)
    {
        if (Held == held) return;
        Held = held;
        hideTimer = 1f;
        if (held) CurMode = Mode.Capture;
    }


    private void Update()
    {
        if (!Held)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0f) gameObject.SetActive(false);
            return;
        }
        if (!heldDevice.isValid) return;

        switch (CurMode)
        {
            case Mode.Capture:
                CaptureBehav.Update(heldDevice); break;
            case Mode.Attach:
                AttachBehav.Update(); break;
        }

    }

    public Mode CurMode { get; private set; } = Mode.Capture;

    public void ChangeMode(Mode newMode)
    {
        if (CurMode == newMode) return;

        switch (CurMode)
        {
            case Mode.Capture:
                break;
            case Mode.Attach:
                break;
        }
        switch (newMode)
        {
            case Mode.Capture:
                break;
            case Mode.Attach:
                break;
        }

        CurMode = newMode;
    }

    public enum Mode
    {
        Capture,
        Attach
    }
}
