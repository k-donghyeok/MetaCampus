using UnityEngine;

public class OneTimeDoor : DoorLock
{
    private Animator animator = null;

    protected void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.OneTime;
        animator = GetComponentInChildren<Animator>();
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
        animator.SetTrigger("isOpen");
        return true;
    }
}
