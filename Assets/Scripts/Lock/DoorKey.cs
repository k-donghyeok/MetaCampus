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
    /// 놓여있을 때 얼마나 더 커보일지
    /// </summary>
    [SerializeField, Range(0.1f, 20.0f)]
    private float dropScale = 1f;

    private float angleDeg = 0f;
    #endregion



    protected virtual void Start()
    {
        LockManager.DyeRenderers(LockColorID, dyeRenderers);
    }

    private void Update()
    {
        if (modelTransform)
        {
            FloatUpdate();
            modelTransform.localScale = Vector3.one * dropScale; // TODO: react to picked up state
        }
    }

    private void FloatUpdate()
    {
        angleDeg = (angleDeg + 60f * Time.deltaTime) % 360f;

        modelTransform.position = new Vector3(modelTransform.position.x,
            Mathf.Sin(Mathf.Deg2Rad * angleDeg) * 0.1f + 0.9f,
            modelTransform.position.z);
        modelTransform.rotation = Quaternion.Euler(0f, angleDeg, 0f);
    }
}
