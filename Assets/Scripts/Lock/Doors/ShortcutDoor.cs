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
        var dot = Vector3.Dot(dir, Clockwise ? transform.forward : -transform.forward);
        if (dot < 0f) return; // Not Front

        IsUnlocked = true;
        PlayOpenAnimation();
    }
}