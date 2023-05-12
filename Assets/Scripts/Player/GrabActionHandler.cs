using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// �κ��丮���� ������ ��� �׼� ó��
/// </summary>
public class GrabActionHandler : MonoBehaviour
{
    [Header("Controller Settings")]
    [SerializeField, Range(0.0f, 1.0f)]
    private float gripThreshold = 0.9f;

    private InputDevice[] targetDevices;

    private void Start()
    {
        InitDevices();
    }

    /// <summary>
    /// ������ VR �Է� ��ġ�� ������
    /// </summary>
    private void InitDevices()
    {
        lastGrabs[0] = false; lastGrabs[1] = false;

        List<InputDevice> devices = new();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left, devices);
        if (devices.Count < 1) return;
        targetDevices[0] = devices[0];

        devices = new();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, devices);
        if (devices.Count < 1) return;
        targetDevices[1] = devices[0];
    }

    /// <summary>
    /// ���� �����ӿ��� ��� ���̾����� ���� (index 0: ����, 1: ������)
    /// </summary>
    private readonly bool[] lastGrabs = new bool[2];

    private void Update()
    {
        if (!targetDevices[0].isValid || !targetDevices[1].isValid)
        { InitDevices(); return; }

        for (int i = 0; i < 2; ++i)
        {
            bool grab = false;
            if (targetDevices[i].TryGetFeatureValue(CommonUsages.grip, out float grip))
                grab = grip > gripThreshold;

            if (grab && !lastGrabs[i]) OnGrabbed?.Invoke(i == 0, targetDevices[i]);
            else if (!grab && lastGrabs[i]) OnGrabReleased?.Invoke(i == 0, targetDevices[i]);

            lastGrabs[i] = grab;
        }
    }

    /// <summary>
    /// ��� �ൿ�� �̺�Ʈ
    /// </summary>
    /// <param name="left">���� ������ ��Ҵ��� �������̾����� ����</param>
    /// <param name="device">��⸦ �� �Է� ��ġ ����</param>
    public delegate void GrabHandler(bool left, InputDevice device);

    /// <summary>
    /// ����� �� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public GrabHandler OnGrabbed;

    /// <summary>
    /// ���� �� ������ �� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public GrabHandler OnGrabReleased;

}
