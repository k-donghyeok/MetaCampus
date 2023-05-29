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
        // 종류가 다름: 아무 일도 일어나지 않음
        if (_collision.LockTypeID != LockTypeID) return;
        // 맞지 않음: 뭔가 이벤트?
        if (_collision.LockColorID != LockColorID)
        {

            return;
        }

        IsOpened = true;
        animator.SetTrigger("isOpen");
        _collision.gameObject.SetActive(false);
        Debug.Log("일회용 열쇠 사용 문이 열림");
    }
}
