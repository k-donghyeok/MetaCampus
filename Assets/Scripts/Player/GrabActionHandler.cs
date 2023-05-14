using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// �κ��丮���� ������ ��� �׼� ó��
/// </summary>
public class GrabActionHandler : MonoBehaviour
{
    [SerializeField]
    internal XRDirectInteractor[] directInteractors = new XRDirectInteractor[2]; 

    [Header("Controller Settings")]
    [SerializeField, Range(0.0f, 1.0f)]
    private float gripThreshold = 0.9f;

    private readonly InputDevice[] targetDevices = new InputDevice[2];

    private void Start()
    {
        InitDevices();
    }

    /// <summary>
    /// ������ VR �Է� ��ġ�� ������
    /// </summary>
    private void InitDevices()
    {
        for (int i = 0; i < 2; ++i)
        {
            lastGrabs[i] = false;

            List<InputDevice> devices = new();
            InputDevices.GetDevicesWithCharacteristics(i == 0 ?
                InputDeviceCharacteristics.Left : InputDeviceCharacteristics.Right, devices);
            if (devices.Count < 1) continue;
            targetDevices[i] = devices[0];
        }
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
            if (grab && !lastGrabs[i] && !GrabOccupied(i)) OnGrabbed?.Invoke(i == 0, targetDevices[i]);
            else if (!grab && lastGrabs[i]) OnGrabReleased?.Invoke(i == 0, targetDevices[i]);

            lastGrabs[i] = grab;
        }
    }

    public bool GrabOccupied(int grasp)
    {
        if (!directInteractors[grasp].attachTransform) return true;
        return directInteractors[grasp].attachTransform.childCount > 0;
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
