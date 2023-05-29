using UnityEngine;

public class OneTimeDoor : DoorLock
{
    private Animator animator = null;

    protected void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.OneTime;
        animator = GetComponent<Animator>();
    }

    protected override void Unlock(DoorKey _collision)
    {
        if (_collision.LockColorID == LockColorID && _collision.LockTypeID == LockTypeID)
        {
            Destroy(_collision.gameObject);
            Debug.Log("��ȸ�� ���� ��� ���̿���");

            animator.SetTrigger("isOpen");
        }
    }
}
