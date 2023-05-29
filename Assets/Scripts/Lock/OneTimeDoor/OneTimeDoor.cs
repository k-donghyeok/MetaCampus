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
            Debug.Log("일회용 열쇠 사용 문이열림");

            animator.SetTrigger("isOpen");
        }
    }
}
