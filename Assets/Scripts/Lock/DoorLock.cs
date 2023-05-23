using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class DoorLock : MonoBehaviour, IHaveLockID
{
    [SerializeField]
    private int lockTypeID = 0;
    [SerializeField]
    private int lockColorID = 0;

    public int LockTypeID => lockTypeID;

    public int  LockColorID => lockColorID;

    public abstract void Unlock(DoorKey _collision);

    private Color[] colors = { new Color(1f, 0f, 0f), new Color(0f, 1f, 0f), new Color(0f, 0f, 1f), new Color(1f, 0.92f, 0.16f), new Color(0f, 1f, 1f) };// 스테이지 매니저에서 가져오는게 좋아보임

    [SerializeField]
    private Color color = Color.magenta;

    protected virtual void Awake()
    {
    }
    protected virtual void Start()
    {
        SetColors(colors);
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        DoorKey go = collision.gameObject.GetComponent<DoorKey>();
        if(go==null)
        {
            Debug.Log("충돌된게 열쇠가 아님");
            return;
        }
        Debug.Log("충돌됨");
        Unlock(go);
    }


    private void SetColors(Color[] _colors)
    {
        Color color = _colors[LockColorID];

        MeshRenderer[] ren = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < ren.Length; ++i)
        {
            Debug.Log($"{i}: {ren[i].material.name}, {ren[i].material.GetColor("_BaseColor")}");
            ren[i].material.SetColor("_BaseColor", color);
        }
    }

}
