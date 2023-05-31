using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ShortcutDoor : DoorLock
{
    [SerializeField]
    private Transform handles = null;

    protected override void Start()
    {
        base.Start();
        if (!Clockwise) handles.transform.Rotate(0f, 180f, 0f, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.root.CompareTag("Player")) return; // Not Player
        var dir = other.transform.position - transform.position;
        var dot = Vector3.Dot(dir, transform.forward);
        if (Clockwise && dot < 0.1f) return; // Not Front
        if (!Clockwise && dot > -0.1f) return;

        IsUnlocked = true;
        PlayOpenAnimation();
    }
}