public class RemoteDoor : DoorLock
{
    private void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.Remote;
    }

    protected override void Start()
    {
        base.Start();

        if (StageManager.Instance().Initialized) AppendEventToLock(StageManager.Instance());
        else StageManager.Instance().OnStageLoad += (stage) => AppendEventToLock(stage);
    }

    private void AppendEventToLock(StageManager stage)
    {
        stage.Lock.OnRemoteOpened += (color) =>
        {
            if (LockColorID != color) return;
            IsUnlocked = true;
            PlayOpenAnimation();
        };
    }
}
