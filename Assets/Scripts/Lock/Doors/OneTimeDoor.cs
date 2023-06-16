using UnityEngine;

public class OneTimeDoor : DoorLock
{
    [SerializeField]
    private AudioClip openSound;
    private AudioSource audioSource;

    protected void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.OneTime;
    }

    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }

    public override bool TryUnlock(DoorKey key)
    {
        // ������ �ٸ�: �ƹ� �ϵ� �Ͼ�� ����
        if (key.LockTypeID != LockTypeID) return false;
        // ���� ����: ���� �̺�Ʈ?
        if (key.LockColorID != LockColorID)
        {

            return false;
        }

        IsUnlocked = true;
        PlayOpenAnimation();
        PlayOpenSound();
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
