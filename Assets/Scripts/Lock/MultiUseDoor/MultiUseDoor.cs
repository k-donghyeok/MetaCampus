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
        // 종류가 다름: 아무 일도 일어나지 않음
        if (_collision.LockTypeID != LockTypeID) return;
        // 맞지 않음: 뭔가 이벤트?
        if (_collision.LockColorID != LockColorID)
        {

            return;
        }

        //문이열린다
        IsOpened = true;
        animator.SetTrigger("isOpen");
        Debug.Log("카드키 사용 문열림");
    }
}
