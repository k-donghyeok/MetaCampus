using UnityEngine;

public class MultiUseDoor : DoorLock
{
    private Animator animator = null;

    protected void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.MultiUse;
        animator = GetComponent<Animator>();
    }

    protected override void Unlock(DoorKey _collision)
    {
        // ������ �ٸ�: �ƹ� �ϵ� �Ͼ�� ����
        if (_collision.LockTypeID != LockTypeID) return;
        // ���� ����: ���� �̺�Ʈ?
        if (_collision.LockColorID != LockColorID)
        {

            return;
        }

        //���̿�����
        IsOpened = true;
        animator.SetTrigger("isOpen");
        Debug.Log("ī��Ű ��� ������");
    }
}
