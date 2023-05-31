using UnityEngine;

public class PasswordDoor : DoorLock
{
    protected void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.Password;
    }

    protected override void Start()
    {
        base.Start();
        StageManager.Instance().OnStageLoad += (stage) => SavePassword(stage);
    }

    private int password;
    private int curInput = 0;

    private void SavePassword(StageManager stage)
    {
        password = stage.Lock.GetPassword(LockColorID);
    }


    public void OnButtonPressed(int number)
    {
        curInput = curInput * 10 + number;
        if (curInput >= 1000) CheckPassword();
    }

    private void CheckPassword()
    {
        if (curInput == password)
        {

        }
        else
        {
            curInput = 0;
        }
    }

}
