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
            //문이열린다
            animator.SetTrigger("isOpen");
            Debug.Log("카드키 사용 문열림");
        }
    }
}
