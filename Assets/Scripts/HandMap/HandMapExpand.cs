using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(GrabActionHandler))]
public class HandMapExpand : MonoBehaviour
{
    [SerializeField]
    private HandMapManager map = null;

    private GrabActionHandler grabActionHandler = null;

    private XRDirectInteractor[] DirectInteractors => grabActionHandler.directInteractors;

    private void Start()
    {
        if (!map)
            throw new ArgumentNullException(nameof(map));
        grabActionHandler = GetComponent<GrabActionHandler>();

        grabActionHandler.OnGrabbed += (left, device) =>
        {
            // if device is high and unoccupied
            // flick a flag for that hand
        };
        grabActionHandler.OnGrabReleased += (left, device) =>
        {
            // flick those flag off
        };
    }

    private void Update()
    {
        // if both flags are enabled, ready for expand
            // if their directinteractors' attachtransform is far enough horizontally, init appear animation and EnableMap()
        // if it's enabled but one of the flag is disabled, move the other transform to another
            // if both flags are disabled, make both of them to move into each other
            // if distance is within limit, execute dissappear animation
            // if the animation is done, DisableMap
    }

    private void EnableMap()
    {
        map.gameObject.SetActive(true);
    }


}
