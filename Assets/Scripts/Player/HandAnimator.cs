using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// ��Ʈ�ѷ� ���ۿ� �°� ���� �ִϸ��̼��� ����
/// </summary>
[RequireComponent(typeof(Animator))]
public class HandAnimator : MonoBehaviour
{
    [SerializeField]
    private InputDeviceCharacteristics inputDeviceCharacteristics;

    private Animator anim;
    private InputDevice targetDevice;

    private void Start()
    {
        anim = GetComponent<Animator>();

        InitDevice();
    }

    private void InitDevice()
    {
        List<InputDevice> devices = new();
        InputDevices.GetDevicesWithCharacteristics(inputDeviceCharacteristics, devices);

        if (devices.Count < 1) return;
        targetDevice = devices[0];
    }

    private void Update()
    {
        if (!targetDevice.isValid) { InitDevice(); return; }

        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            anim.SetFloat("Trigger", triggerValue);
        else
            anim.SetFloat("Trigger", 0f);

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            anim.SetFloat("Grip", gripValue);
        else
            anim.SetFloat("Grip", 0f);
    }
}