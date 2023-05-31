using UnityEngine;

public class OneTimeDoor : DoorLock
{

    protected void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.OneTime;
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
        return true;
    }
}
