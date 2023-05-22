using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DoorKey : MonoBehaviour, IHaveLockID
{
    [SerializeField]
    private int lockID = 0;

    public int LockID => lockID;

    #region 떠다니는데 필요한 변수
    [SerializeField,Range(1f,10f)]private float floatspeed = 1f;
    [SerializeField, Range(0f, 10f)] private float height = 1f;
    private float angle = 0f;
    #endregion

    private void Update()
    {
        Float();
    }
    private void Float()
    {
        angle += floatspeed;
        if(angle>360f)
        {
            angle = 0f;
        }
        float deg = Mathf.Deg2Rad * angle;
        float sin = Mathf.Sin(deg);
       
        transform.position = new Vector3(transform.position.x, height + sin, transform.position.z);
    }
}
