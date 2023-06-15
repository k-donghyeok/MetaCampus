using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RemoteWheel : RemoteKeyBase
{
    [SerializeField]
    private XRBaseInteractable interactable;

    [SerializeField]
    private Transform wheel;

    [SerializeField, Range(1, 20)]
    private int spinAmount = 5;

    [SerializeField]
    private TMP_Text txtLeft;

    protected override void Start()
    {
        base.Start();

        txtLeft.text = spinAmount.ToString();
    }

    private int spinDone = 0;
    private float lastDeg = 0f;

    protected override void Update()
    {
        if (!Held) return;
        var hand = interactable.firstInteractorSelecting;
        if (hand == null) return;
        Vector3 dir = hand.transform.position - wheel.position;
        if ((hand.transform.position - (wheel.position + wheel.up * 0.45f)).magnitude > 0.4f) // Too far
        { interactable.enabled = false; interactable.enabled = true; return; }
        //float deg = Vector2.SignedAngle(transform.up, dir);
        //deg = Mathf.LerpAngle(lastDeg, deg, 0.5f);
        //wheel.rotation = Quaternion.Euler(0f, 0f, deg);
        Quaternion parentRotation = transform.rotation;
        Vector3 localDir = Quaternion.Inverse(parentRotation) * dir; // Transform dir to local space

        float deg = Vector2.SignedAngle(transform.up, localDir);
        deg = Mathf.LerpAngle(lastDeg, deg, 0.5f);

        // Apply the rotation with parent's rotation taken into account
        wheel.rotation = parentRotation * Quaternion.Euler(0f, 0f, deg);

        lastDeg = deg;
        deg /= 360f;
        int spin = Mathf.FloorToInt(deg) + (deg < 0 && deg != Mathf.Floor(deg) ? 1 : 0);
        if (spinDone != spin)
        {
            spinDone = spin;
            int spinLeft = spinAmount - Mathf.Abs(spinDone);
            txtLeft.text = spinLeft.ToString();
            if (spinLeft == 0)
            {
                txtLeft.color = LockManager.GetColor(LockColorID);
                interactable.enabled = false;
                OnUsed();
            }
        }
    }

    public override void OnHeld()
    {
        base.OnHeld();
    }

    public override void OnHeldReleased()
    {
        base.OnHeldReleased();
    }

    protected override void FloatUpdate()
    {
    }

}