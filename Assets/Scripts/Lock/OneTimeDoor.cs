using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class OneTimeDoor : DoorLock
{

    private bool isOpen = false;
    private float y = 0f;

    [SerializeField,Range(-10f,10f)] private float rotateSpeed = 1f;
    public override void Unlock(DoorKey _collision)
    {
       if(_collision.LockID == LockID)
       {
            Destroy(_collision.gameObject);
            Debug.Log("일회용 열쇠 사용 문이열림");
            
            isOpen = true;
       }
    }

    private void Update()
    {
        if(isOpen)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        y += rotateSpeed;
        if(y>90f)
        {
            y = 90f;
            isOpen = false;
        }

        if(y<-90f)
        {
            y = -90f;
            isOpen = false;
        }
        transform.rotation = Quaternion.Euler(0f,y,0f);
    }


}
