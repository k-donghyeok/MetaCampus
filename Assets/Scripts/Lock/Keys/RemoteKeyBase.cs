using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public abstract class RemoteKeyBase : DoorKey
{
    protected bool spent = false;

    private void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.Remote;
        sqrdInteractionDistance = 1.0f;
    }

    protected override void Update()
    {
    }

    public override void OnTrigger()
    {
        OnUsed();
    }

    private void SendOpen()
        => StageManager.Instance().Lock.OpenRemote(LockColorID);

    protected override void OnUsed()
    {
        if (spent) return;
        SendOpen();
        spent = true;
    }
}