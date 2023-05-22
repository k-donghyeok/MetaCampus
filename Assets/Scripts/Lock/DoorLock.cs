using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class DoorLock : MonoBehaviour, IHaveLockID
{
    [SerializeField]
    private int lockID = 0;

    public int LockID => lockID;
    public abstract void Unlock(DoorKey _collision);
   
    
    private void OnTriggerEnter(Collider collision)
    {
        DoorKey go = collision.gameObject.GetComponent<DoorKey>();
        if(go==null)
        {
            Debug.Log("�浹�Ȱ� ���谡 �ƴ�");
            return;
        }
        Debug.Log("�浹��");
        Unlock(go);
    }

  

}
