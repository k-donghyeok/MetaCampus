using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeKey : DoorKey
{
    private void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.OneTime;
    }

    protected override void OnUsed()
    {
        base.OnUsed();
        gameObject.SetActive(false);
        // TODO: ������� �ִϸ��̼� �߰�?
    }
}
