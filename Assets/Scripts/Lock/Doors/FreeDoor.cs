using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FreeDoor : DoorLock
{
    public void Toggle(XRBaseInteractable self)
    {
        var interactor = self.GetOldestInteractorHovering();
        if (interactor == null) return;
        var go = interactor.transform.root;
        var pm = go.GetComponent<PlayerManager>();
        if (pm == null) return;

        /*
        var dir = pm.xrOrigin.position - transform.position;
        var dot = Vector3.Dot(dir, Clockwise ? transform.forward : -transform.forward);
        if (dot < 0f) return; // Not Front

        IsUnlocked = true;
        PlayOpenAnimation();
        self.enabled = false; */
    }
}