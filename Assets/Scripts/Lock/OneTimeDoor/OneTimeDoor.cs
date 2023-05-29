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
        // ������ �ٸ�: �ƹ� �ϵ� �Ͼ�� ����
        if (_collision.LockTypeID != LockTypeID) return;
        // ���� ����: ���� �̺�Ʈ?
        if (_collision.LockColorID != LockColorID)
        {

            return;
        }

        IsOpened = true;
        animator.SetTrigger("isOpen");
        _collision.gameObject.SetActive(false);
        Debug.Log("��ȸ�� ���� ��� ���� ����");
    }
}
