using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DoorLock : MonoBehaviour, IHaveLockID
{
    [SerializeField]
    private int lockID = 0;

    public int LockID => lockID;

}
