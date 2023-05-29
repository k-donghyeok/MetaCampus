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
        if (_collision.LockColorID == LockColorID && _collision.LockTypeID == LockTypeID)
        {
            //���̿�����
            animator.SetTrigger("isOpen");
            Debug.Log("ī��Ű ��� ������");
        }
    }
}
