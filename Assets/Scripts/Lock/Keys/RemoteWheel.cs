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

    private int spinLeft;

    protected override void Start()
    {
        base.Start();

        spinLeft = spinAmount;
        txtLeft.text = spinLeft.ToString();
    }

    private float lastDeg = 0f;

    protected override void Update()
    {
        if (!Held) return;
        var hand = interactable.firstInteractorSelecting;
        if (hand == null) return;
        Vector2 dir = hand.transform.position - wheel.position;
        float deg = Vector2.SignedAngle(transform.up, dir);
        //deg = Mathf.Lerp(lastDeg, deg, 0.2f);
        wheel.rotation = Quaternion.Euler(0f, 0f, deg);

        lastDeg = deg;
    }

    public override void OnHeldReleased()
    {
        base.OnHeldReleased();
    }

    protected override void FloatUpdate()
    {
    }

}