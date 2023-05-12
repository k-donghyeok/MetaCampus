using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool Held
    {
        get => _held;
        set { if (_held == value) return; _held = value; }
    }
    private bool _held;

    private void Update()
    {
        if (!Held) return;
    }

}
