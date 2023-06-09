using UnityEngine;

public class MultiUseDoor : DoorLock
{
    [SerializeField]
    private AudioClip openSound;
    private AudioSource audioSource;

    protected void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.MultiUse;
    }

    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }

    public override bool TryUnlock(DoorKey key)
    {
        // 종류가 다름: 아무 일도 일어나지 않음
        if (key.LockTypeID != LockTypeID) return false;
        // 맞지 않음: 뭔가 이벤트?
        if (key.LockColorID != LockColorID)
        {

            return false;
        }

        //문이열린다
        IsUnlocked = true;
        PlayOpenAnimation();
        PlayOpenSound();
        //Debug.Log("카드키 사용 문열림");
        return true;
    }

    private void PlayOpenSound()
    {
        if (openSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(openSound);
        }

    }
}
