using UnityEngine;

public class PasswordDoor : DoorLock
{
    private bool isOpen = false;
    private float y = 0f;

    protected void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.Password;
    }

    [SerializeField, Range(-10f, 10f)]
    private float rotateSpeed = 1f;


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
