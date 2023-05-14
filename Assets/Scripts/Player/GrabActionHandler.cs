using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// 인벤토리에서 꺼내는 잡기 액션 처리
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
    /// 추적할 VR 입력 장치를 가져옴
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
    /// 이전 프레임에서 잡기 중이었는지 저장 (index 0: 왼쪽, 1: 오른쪽)
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
    /// 잡기 행동의 이벤트
    /// </summary>
    /// <param name="left">왼쪽 손으로 잡았는지 오른손이었는지 여부</param>
    /// <param name="device">잡기를 한 입력 장치 정보</param>
    public delegate void GrabHandler(bool left, InputDevice device);

    /// <summary>
    /// 잡았을 때 발생하는 이벤트
    /// </summary>
    public GrabHandler OnGrabbed;

    /// <summary>
    /// 잡은 걸 놓았을 때 발생하는 이벤트
    /// </summary>
    public GrabHandler OnGrabReleased;

}
