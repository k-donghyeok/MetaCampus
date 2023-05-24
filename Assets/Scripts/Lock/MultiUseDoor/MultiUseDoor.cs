using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiUseDoor : DoorLock
{
    private Animator animator = null;

    protected override void Awake()
    {
        base.Awake();
        lockTypeID = IHaveLockID.TypeID.MultiUse;

        animator = GetComponent<Animator>();
    }

    public override void Unlock(DoorKey _collision)
    {
       if(_collision.LockColorID == LockColorID && _collision.LockTypeID ==LockTypeID )
       {
            //���̿�����
            animator.SetTrigger("isOpen");
            Debug.Log("ī��Ű ��� ������");
       }
    }
}
