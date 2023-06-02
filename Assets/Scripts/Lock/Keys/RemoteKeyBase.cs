public abstract class RemoteKeyBase : DoorKey
{
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
}