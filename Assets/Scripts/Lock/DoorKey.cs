using UnityEngine;
using static IHaveLockID;

public abstract class DoorKey : MonoBehaviour, IHaveLockID
{
    [SerializeField]
    private ColorID lockColorID = ColorID.Red;

    protected TypeID lockTypeID = TypeID.None;

    public TypeID LockTypeID => lockTypeID;
    public ColorID LockColorID => lockColorID;

    #region Visual
    /// <summary>
    /// 부유 속도
    /// </summary>
    [SerializeField, Range(1f, 10f)]
    private float floatspeed = 1f;
    /// <summary>
    /// 부유 왕복 높이
    /// </summary>
    [SerializeField, Range(0f, 10f)]
    private float height = 1f;
    /// <summary>
    /// 놓여있을 때 얼마나 더 커보일지
    /// </summary>
    [SerializeField, Range(0.1f, 20.0f)]
    private float dropScale = 1f;

    private float angle = 0f;
    #endregion



    protected virtual void Start()
    {
        SetColors();
    }
    private void Update()
    {
        Float();
    }
    private void Float()
    {
        angle += floatspeed;
        if (angle > 360f)
        {
            angle = 0f;
        }
        float deg = Mathf.Deg2Rad * angle;
        float sin = Mathf.Sin(deg);

        transform.position = new Vector3(transform.position.x, height + sin, transform.position.z);
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
