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
    [SerializeField]
    private MeshRenderer[] dyeRenderers = new MeshRenderer[0];

    [SerializeField]
    private Transform modelTransform = null;
    /// <summary>
    /// �������� �� �󸶳� �� Ŀ������
    /// </summary>
    [SerializeField, Range(0.1f, 20.0f)]
    private float dropScale = 1f;

    private float angleDeg = 0f;
    #endregion



    protected virtual void Start()
    {
        LockManager.DyeRenderers(LockColorID, dyeRenderers);
        modelTransform.localScale = Vector3.one * dropScale;
        FloatUpdate();
    }

    private void Update()
    {
        if (!Held) FloatUpdate();
    }

    private void FloatUpdate()
    {
        if (!modelTransform) return;
        angleDeg = (angleDeg + 60f * Time.deltaTime) % 360f;

        modelTransform.position = new Vector3(modelTransform.position.x,
            Mathf.Sin(Mathf.Deg2Rad * angleDeg) * 0.1f + 0.9f,
            modelTransform.position.z);
        modelTransform.rotation = Quaternion.Euler(0f, angleDeg, 0f);
    }

    protected bool Held { get; private set; } = false;

    public virtual void OnHeld()
    {
        Held = true;
        modelTransform.localScale = Vector3.one;
        modelTransform.rotation = Quaternion.identity;
        modelTransform.position = Vector3.zero;
    }

    public virtual void OnHeldReleased()
    {
        Held = false;
        modelTransform.localScale = Vector3.one * dropScale;
        FloatUpdate();
    }
}
