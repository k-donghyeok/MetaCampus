public class RemoteDoor : DoorLock
{
    private void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.Remote;
    }

    protected override void Start()
    {
        base.Start();
        StageManager.Instance().OnStageLoad += (stage) =>
        {
            stage.Lock.OnRemoteOpened += (color) =>
            {
                if (LockColorID != color) return;
                IsUnlocked = true;
                PlayOpenAnimation();
            };
        };
    }
}
