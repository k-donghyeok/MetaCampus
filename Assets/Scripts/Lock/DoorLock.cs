using UnityEngine;
using static IHaveLockID;

public abstract class DoorLock : MonoBehaviour, IHaveLockID
{
    [SerializeField]
    private ColorID lockColorID = 0;

    protected TypeID lockTypeID = TypeID.None;

    public TypeID LockTypeID => lockTypeID;

    public ColorID LockColorID => lockColorID;

    public abstract void Unlock(DoorKey _collision);


    protected virtual void Awake()
    {
    }
    protected virtual void Start()
    {
        SetColors();

    }
    private void OnTriggerEnter(Collider collision)
    {
        DoorKey go = collision.gameObject.GetComponent<DoorKey>();
        if (go == null)
        {
            Debug.Log("Ãæµ¹µÈ°Ô ¿­¼è°¡ ¾Æ´Ô");
            return;
        }
        Debug.Log("Ãæµ¹µÊ");
        Unlock(go);
    }


    private void SetColors()
    {
        Color color = LockManager.GetColor(LockColorID);

        MeshRenderer[] ren = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < ren.Length; ++i)
        {
            //Debug.Log($"{i}: {ren[i].material.name}, {ren[i].material.GetColor("_BaseColor")}");
            ren[i].material.SetColor("_BaseColor", color);
        }
    }

}
