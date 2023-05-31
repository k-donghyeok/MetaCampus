using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordDoor : DoorLock
{
    [SerializeField]
    private TMP_Text[] texts = new TMP_Text[2];

    protected void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.Password;
    }

    protected override void Start()
    {
        base.Start();
        StageManager.Instance().OnStageLoad += (stage) => SavePassword(stage);
        foreach (var text in texts) text.SetText(PW_EMPTY);
    }

    private int password;
    private int curInput = 0;
    private const string PW_EMPTY = "XXXX";

    private void SavePassword(StageManager stage)
    {
        password = stage.Lock.GetPassword(LockColorID);
    }


    public void OnButtonPressed(Button button)
    {
        if (IsUnlocked) return; // Already unlocked

        var player = GameObject.FindGameObjectWithTag("Player");
        var canvas = button.transform.parent;
        var dir = player.transform.position - canvas.position;
        if (Vector3.Dot(dir, canvas.forward) < 0f) return; // Player is behind the door

        int number = button.gameObject.name[^1] - '0';
        curInput = curInput * 10 + number;
        if (curInput >= 1000) CheckPassword();
        foreach (var text in texts)
            text.SetText(curInput == 0 ? PW_EMPTY : curInput.ToString());
    }

    private void CheckPassword()
    {
        if (curInput == password)
        {
            IsUnlocked = true;
            PlayOpenAnimation();
        }
        else
        {
            curInput = 0;
        }
    }

}
