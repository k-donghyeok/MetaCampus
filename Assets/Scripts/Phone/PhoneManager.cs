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
    /// �̰��� �÷��̾� �տ� ����ְ�, ��ȣ�ۿ��� �������� ����
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
