using UnityEngine;

public class MultiUseDoor : DoorLock
{
    protected void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.MultiUse;
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
        //Debug.Log("ī��Ű ��� ������");
        return true;
    }
}
