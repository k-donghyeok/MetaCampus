using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiUseDoor : DoorLock
{
    public override void Unlock(DoorKey _collision)
    {
       if(_collision.LockID == LockID)
       {
            //���̿�����
            Debug.Log("ī��Ű ��� ������");
       }
    }
}
