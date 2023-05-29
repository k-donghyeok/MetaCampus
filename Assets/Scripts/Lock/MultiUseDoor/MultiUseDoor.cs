using UnityEngine;

public class MultiUseDoor : DoorLock
{
    private Animator animator = null;

    protected void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.MultiUse;
        animator = GetComponent<Animator>();
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
        animator.SetTrigger("isOpen");
        Debug.Log("ī��Ű ��� ������");
        return true;
    }
}
