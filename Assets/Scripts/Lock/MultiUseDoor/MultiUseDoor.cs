using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiUseDoor : DoorLock
{
    private Animator animator = null;

    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
    }

    public override void Unlock(DoorKey _collision)
    {
       if(_collision.LockID == LockID)
       {
            //문이열린다
            animator.SetTrigger("isOpen");
            Debug.Log("카드키 사용 문열림");
       }
    }
}
