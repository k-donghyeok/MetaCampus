using Unity.VisualScripting;
using UnityEngine;
using static IHaveLockID;

public abstract class DoorLock : MonoBehaviour, IHaveLockID
{
    [SerializeField]
    private ColorID lockColorID = 0;

    [SerializeField]
    private MeshRenderer[] dyeRenderers = new MeshRenderer[0];

    [SerializeField]
    private bool clockwise = true;

    protected TypeID lockTypeID = TypeID.None;

    public TypeID LockTypeID => lockTypeID;

    public ColorID LockColorID => lockColorID;

    protected Animator animator = null;

    /// <summary>
    /// 문을 잠금해제 시도
    /// </summary>
    public virtual bool TryUnlock(DoorKey key)
    {
        return false;
    }

    /// <summary>
    /// 문이 시계방향으로 열리는지 반시계방향으로 열리는지
    /// </summary>
    public bool Clockwise => clockwise;

    /// <summary>
    /// 문이 잠금 해제 상태인지
    /// </summary>
    public bool IsUnlocked { get; protected set; } = false;


    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        LockManager.DyeRenderers(LockColorID, dyeRenderers);
    }

    protected void PlayOpenAnimation()
    {
        if (animator) animator.SetTrigger("isOpen");
    }
}
