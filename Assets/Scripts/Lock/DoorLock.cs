using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class DoorLock : MonoBehaviour, IHaveLockID
{
    [SerializeField]
    private int lockID = 0;

    public int LockID => lockID;

    private void OnCollisionEnter(Collision collision)
    {
        DoorKey a = collision.gameObject.GetComponent<DoorKey>();
        if (a.LockID == LockID)
        {
            Destroy(collision.gameObject);
        }
    }

    

}
