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
    /// ���� �ð�������� �������� �ݽð�������� ��������
    /// </summary>
    public bool Clockwise => clockwise;

    /// <summary>
    /// ���� ���� ��������
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
            Debug.Log("�浹�Ȱ� ���谡 �ƴ�");
            return;
        }
        Debug.Log("����� �浹");
        if (IsOpened) return; // �̹� ���� ����
        Unlock(key);
    }

}
