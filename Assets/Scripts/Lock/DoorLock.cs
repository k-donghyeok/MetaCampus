using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class DoorLock : MonoBehaviour, IHaveLockID
{
    [SerializeField]
    private int lockID = 0;

    public int LockID => lockID;
    public abstract void Unlock(DoorKey _collision);

    [SerializeField]
    private Color color = Color.magenta;

    protected virtual void Awake()
    {
    }
    protected virtual void Start()
    {
        SetColors(color);
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        DoorKey go = collision.gameObject.GetComponent<DoorKey>();
        if(go==null)
        {
            Debug.Log("Ãæµ¹µÈ°Ô ¿­¼è°¡ ¾Æ´Ô");
            return;
        }
        Debug.Log("Ãæµ¹µÊ");
        Unlock(go);
    }
   

    private void SetColors(Color color)
    {
        MeshRenderer[] ren = GetComponentsInChildren<MeshRenderer>();
        for (int i=0;i<ren.Length;++i)
        {
            Debug.Log($"{i}: {ren[i].material.name}, {ren[i].material.GetColor("_BaseColor")}");
            ren[i].material.SetColor("_BaseColor", color);
        }
    }

}
