using System;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// 핸드폰을 꺼내는 행동 처리하는 컴포넌트
/// </summary>
[RequireComponent(typeof(GrabActionHandler))]
public class PhoneTakeOut : MonoBehaviour
{
    [SerializeField]
    private PhoneManager phone = null;

    private GrabActionHandler grabActionHandler = null;

    private XRDirectInteractor[] DirectInteractors => grabActionHandler.directInteractors;

    private void Start()
    {
        if (!phone)
            throw new ArgumentNullException(nameof(phone));
        grabActionHandler = GetComponent<GrabActionHandler>();
        phoneHand = null;

        grabActionHandler.OnGrabbed += (left, device) =>
        {
            if (phoneHand.HasValue) return;
            if (!device.TryGetFeatureValue(CommonUsages.devicePosition, out var pos)) return;
            if (pos.y > 0.5f) return; // too high

            TakeOutPhone(left, device);
        };
        grabActionHandler.OnGrabReleased += (left, device) =>
        {
            if (phoneHand != left) return;
            ReleasePhone();
        };
    }

    private bool? phoneHand = null;

    private void TakeOutPhone(bool left, InputDevice device)
    {
        phoneHand = left;
        phone.gameObject.SetActive(true);
        phone.SetHeld(device);
        phone.transform.SetParent(DirectInteractors[left ? 0 : 1].attachTransform);
        phone.transform.SetLocalPositionAndRotation(Vector3.zero, left ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f));
        phone.GetComponent<Rigidbody>().useGravity = false;
        phone.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void ReleasePhone()
    {
        phoneHand = null;
        phone.transform.SetParent(null);
        phone.GetComponent<Rigidbody>().useGravity = true;
        phone.SetHeld(false);
    }
}
