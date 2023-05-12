using System;
using UnityEngine;
using UnityEngine.XR;

public class PhoneTakeOut : MonoBehaviour
{
    [SerializeField]
    private GrabActionHandler grabActionHandler = null;

    [SerializeField]
    private PhoneManager phone = null;

    private void Start()
    {
        if (!grabActionHandler)
            throw new ArgumentNullException(nameof(grabActionHandler));
        if (!phone)
            throw new ArgumentNullException(nameof(phone));

        grabActionHandler.OnGrabbed += (left, device) =>
        {
            if (phoneHand.HasValue) return;

            if (!device.TryGetFeatureValue(CommonUsages.devicePosition, out var pos)) return;
            Debug.Log(pos);

            TakeOutPhone(left);
        };
        grabActionHandler.OnGrabReleased += (left, device) =>
        {
            if (phoneHand != left) return;
            ReleasePhone();
        };
    }

    private bool? phoneHand = null;

    private void TakeOutPhone(bool left)
    {
        phoneHand = left;
    }

    private void ReleasePhone()
    {
        phoneHand = null;
    }
}
