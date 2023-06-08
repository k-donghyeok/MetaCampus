using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class RemoteWheel : RemoteKeyBase
{
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

    protected override void Update()
    {
        if (!Held) return;
    }

    public override void OnHeldReleased()
    {
        base.OnHeldReleased();
    }

    protected override void FloatUpdate()
    {
    }

}