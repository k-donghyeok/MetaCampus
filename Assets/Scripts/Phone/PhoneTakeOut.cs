using System;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// 핸드폰을 꺼내는 행동 처리하는 컴포넌트
/// </summary>
public class PhoneTakeOut : MonoBehaviour
{
    [SerializeField]
    private GrabActionHandler grabActionHandler = null;

    [SerializeField]
    private PhoneManager phone = null;

    [SerializeField]
    private XRDirectInteractor[] directInteractors = new XRDirectInteractor[2];

    private void Start()
    {
        if (!grabActionHandler)
            throw new ArgumentNullException(nameof(grabActionHandler));
        if (!phone)
            throw new ArgumentNullException(nameof(phone));
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
        phone.transform.SetParent(directInteractors[left ? 0 : 1].attachTransform);
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
