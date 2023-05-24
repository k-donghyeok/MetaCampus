using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordDoor : DoorLock
{
    private bool isOpen = false;
    private float y = 0f;

    protected override void Awake()
    {
        base.Awake();
        lockTypeID = IHaveLockID.TypeID.Password;
    }

    [SerializeField, Range(-10f, 10f)] private float rotateSpeed = 1f;
    public override void Unlock(DoorKey _collision)
    {
        
    }

    private void Update()
    {
        if (isOpen)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        y += rotateSpeed;
        if (y > 90f)
        {
            y = 90f;
            isOpen = false;
        }

        if (y < -90f)
        {
            y = -90f;
            isOpen = false;
        }
        transform.rotation = Quaternion.Euler(0f, y, 0f);
    }
}
