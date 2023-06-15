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

        //���̿�����
        IsUnlocked = true;
        PlayOpenAnimation();
        PlayOpenSound();
        //Debug.Log("ī��Ű ��� ������");
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
