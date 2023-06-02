using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public abstract class RemoteKeyBase : DoorKey
{
    [SerializeField]
    protected XRGrabInteractable interactable;

    protected bool spent = false;

    private void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.Remote;
    }

    protected override void Update()
    {
    }

    public override void OnTrigger()
    {
    }

    protected void SendOpen()
        => StageManager.Instance().Lock.OpenRemote(LockColorID);

    protected override void OnUsed()
    {
        spent = true;
        var rBody = interactable.GetComponent<Rigidbody>();
        rBody.velocity = Vector3.zero;
        rBody.constraints = RigidbodyConstraints.FreezeAll;
        interactable.enabled = false;
    }
}