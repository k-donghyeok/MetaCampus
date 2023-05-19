using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// �ڵ����� �������� �ൿ ����
/// </summary>
public class PhoneManager : MonoBehaviour
{
    [SerializeField]
    private Camera cam = null;

    [SerializeField]
    private MeshRenderer screen = null;

    [SerializeField]
    private Canvas canvas = null;

    public Canvas Canvas() => canvas;

    public PhotoManager PhotoMgr { get; private set; } = null;
    public CaptureBehaviour CaptureBehav { get; private set; } = null;

    private void Awake()
    {
        PhotoMgr = new PhotoManager(this);
        CaptureBehav = new CaptureBehaviour(this, cam);
    }

    /// <summary>
    /// �̰��� �÷��̾� �տ� ����ְ�, ��ȣ�ۿ��� �������� ����
    /// </summary>
    public bool Held { get; private set; }

    private float hideTimer = 1f;

    private InputDevice heldDevice;

    /// <summary>
    /// �ڵ����� ���� ��Ʈ�ѷ� ����
    /// </summary>
    /// <param name="device"></param>
    public void SetHeld(InputDevice device)
    {
        SetHeld(true);
        heldDevice = device;
    }

    /// <summary>
    /// �ڵ����� ����
    /// </summary>
    /// <param name="held">�ڵ����� ���� ����</param>
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
        if (!heldDevice.isValid) return;

        CaptureBehav.Update(heldDevice);

        

    }

}
