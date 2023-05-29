using UnityEngine;
using static IHaveLockID;

public abstract class DoorLock : MonoBehaviour, IHaveLockID
{
    [SerializeField]
    private ColorID lockColorID = 0;

    protected TypeID lockTypeID = TypeID.None;

    public TypeID LockTypeID => lockTypeID;

    public ColorID LockColorID => lockColorID;

    protected abstract void Unlock(DoorKey _collision);

    [SerializeField]
    private MeshRenderer[] dyeRenderers = new MeshRenderer[0];

    [SerializeField]
    private bool clockwise = true;

    /// <summary>
    /// 문이 시계방향으로 열리는지 반시계방향으로 열리는지
    /// </summary>
    public bool Clockwise => clockwise;

    /// <summary>
    /// 문이 열린 상태인지
    /// </summary>
    public bool IsOpened { get; protected set; } = false;

    protected virtual void Start()
    {
        LockManager.DyeRenderers(LockColorID, dyeRenderers);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.TryGetComponent<DoorKey>(out var key))
        {
            Debug.Log("충돌된게 열쇠가 아님");
            return;
        }
        Debug.Log("열쇠와 충돌");
        if (IsOpened) return; // 이미 열린 상태
        Unlock(key);
    }

}
