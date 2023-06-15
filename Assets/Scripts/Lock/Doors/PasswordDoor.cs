using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PasswordDoor : DoorLock
{
    [SerializeField]
    private TMP_Text[] texts = new TMP_Text[2];

    [SerializeField]
    private AudioClip buttonClickSound;
    [SerializeField]
    private AudioClip openSound;
    private AudioSource audioSource;


    protected void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.Password;
    }

    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = openSound;
        if (StageManager.Instance().Initialized) SavePassword(StageManager.Instance());
        else StageManager.Instance().OnStageLoad += (stage) => SavePassword(stage);

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

        var canvas = button.transform.parent;

        var dir = PlayerManager.InstanceOrigin().position - canvas.position;
        //Debug.Log($"{gameObject.name} OnButtonPressed: {PlayerManager.InstanceOrigin().position}/{canvas.position} {Vector3.Dot(dir, -canvas.forward)}");
        if (Vector3.Dot(dir, -canvas.forward) < 0f) return; // Player is behind the door

        int number = button.gameObject.name[^1] - '0';
        curInput = curInput * 10 + number;
        if (curInput >= 1000) CheckPassword();
        foreach (var text in texts)
            text.SetText(curInput == 0 ? PW_EMPTY : curInput.ToString());

        PlayButtonClickSound();
    }

    private void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
            audioSource.PlayOneShot(buttonClickSound);
    }

    private void CheckPassword()
    {
        if (curInput == password)
        {
            IsUnlocked = true;
            var buttons = GetComponentsInChildren<Button>();
            foreach (var b in buttons) b.interactable = false;
            PlayOpenAnimation();
            PlayOpenSound();
        }
        else
        {
            curInput = 0;
        }
    }

    private void PlayOpenSound()
    {
        if (openSound != null)
            audioSource.PlayOneShot(openSound);
    }
}
