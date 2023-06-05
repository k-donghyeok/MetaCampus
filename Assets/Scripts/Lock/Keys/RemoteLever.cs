using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RemoteLever : RemoteKeyBase
{
    public override void OnTrigger()
    {
        if (spent) return;
        base.OnTrigger();

        GetComponent<Animator>().SetTrigger("Used");
        GetComponent<XRSimpleInteractable>().enabled = false;
    }
}