using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiUseDoor : DoorLock
{
    public override void Unlock(DoorKey _collision)
    {
       if(_collision.LockID == LockID)
       {
            //문이열린다
            Debug.Log("카드키 사용 문열림");
       }
    }
}
