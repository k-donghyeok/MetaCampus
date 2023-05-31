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
    /// ���� ������� �õ�
    /// </summary>
    public virtual bool TryUnlock(DoorKey key)
    {
        return false;
    }

    /// <summary>
    /// ���� �ð�������� �������� �ݽð�������� ��������
    /// </summary>
    public bool Clockwise => clockwise;

    /// <summary>
    /// ���� ��� ���� ��������
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
