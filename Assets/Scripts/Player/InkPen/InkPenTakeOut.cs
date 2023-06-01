using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// ÆæÀ» ²¨³»´Â Çàµ¿ Ã³¸®
/// </summary>
[RequireComponent(typeof(GrabActionHandler))]
public class InkPenTakeOut : MonoBehaviour
{
    [SerializeField]
    private InkPenManager pen = null;
    private GrabActionHandler grabActionHandler = null;

    private XRDirectInteractor[] DirectInteractors => grabActionHandler.directInteractors;

    private void Start()
    {
        if (!pen)
            throw new ArgumentNullException(nameof(pen));
        grabActionHandler = GetComponent<GrabActionHandler>();
        penHand = null;

        grabActionHandler.OnGrabbed += (left, device) =>
        {
            if (penHand.HasValue || grabActionHandler.GrabOccupied(left)) return;
            if (!device.TryGetFeatureValue(CommonUsages.devicePosition, out var pos)) return;
            if (pos.y < 1.0f) return; // too low

            float front = 0.1f;
            { // Head Pos Check
                List<InputDevice> devices = new();
                InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeadMounted, devices);
                if (devices.Count < 1) goto FrontCheck;
                if (!devices[0].TryGetFeatureValue(CommonUsages.devicePosition, out var headPos)) goto FrontCheck;
                front += headPos.z;
            }
            FrontCheck: 
            if (pos.z > front) return; // too front

            TakeOutPen(left, device);
        };
        grabActionHandler.OnGrabReleased += (left, device) =>
        {
            if (penHand != left) return;
            ReleasePen();
        };
    }

    private bool? penHand = null;

    private void TakeOutPen(bool left, InputDevice device)
    {
        penHand = left;
        pen.gameObject.SetActive(true);
        pen.SetHeld(device);
        pen.transform.SetParent(DirectInteractors[left ? 0 : 1].attachTransform);
        grabActionHandler.RequestHandAnimation(left, HandAnimator.SpecialAnimation.GripPen);
        if (left)
            pen.transform.SetLocalPositionAndRotation(Vector3.right * 0.05f, Quaternion.identity);
        else
            pen.transform.SetLocalPositionAndRotation(Vector3.left * 0.05f, Quaternion.identity);
        pen.GetComponent<Rigidbody>().useGravity = false;
        pen.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void ReleasePen()
    {
        grabActionHandler.RequestHandAnimation(penHand.Value, HandAnimator.SpecialAnimation.None);
        penHand = null;
        pen.transform.SetParent(null);
        pen.GetComponent<Rigidbody>().useGravity = true;
        pen.SetHeld(false);
    }
}
