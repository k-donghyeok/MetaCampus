using UnityEngine;
using static IHaveLockID;

public abstract class DoorKey : MonoBehaviour, IHaveLockID
{
    [SerializeField]
    private ColorID lockColorID = ColorID.Red;

    protected TypeID lockTypeID = TypeID.None;

    public TypeID LockTypeID => lockTypeID;
    public ColorID LockColorID => lockColorID;

    [SerializeField]
    private MeshRenderer[] dyeRenderers = new MeshRenderer[0];

    [SerializeField]
    private Transform groundModel = null;
    [SerializeField]
    private Transform heldModel = null;

    private float angleDeg = 0f;


    protected virtual void Start()
    {
        LockManager.DyeRenderers(LockColorID, dyeRenderers);
        groundModel.gameObject.SetActive(true);
        heldModel.gameObject.SetActive(false);

        FloatUpdate();
    }

    private void Update()
    {
        if (!Held) FloatUpdate();
    }

    private void FloatUpdate()
    {
        if (!groundModel) return;
        angleDeg = (angleDeg + 60f * Time.deltaTime) % 360f;

        groundModel.localPosition = new Vector3(groundModel.localPosition.x,
            Mathf.Sin(Mathf.Deg2Rad * angleDeg) * 0.1f,
            groundModel.localPosition.z);
        groundModel.rotation = Quaternion.Euler(0f, angleDeg, 0f);
    }

    protected bool Held
    {
        get => held;
        private set
        {
            if (held == value) return;
            held = value;
            GetComponent<CapsuleCollider>().enabled = !held;
            groundModel.gameObject.SetActive(!held);
            heldModel.gameObject.SetActive(held);
        }
    }
    private bool held = false;

    public virtual void OnHeld()
    {
        Held = true;
    }

    public virtual void OnHeldReleased()
    {
        Held = false;
        transform.rotation = Quaternion.identity;
        FloatUpdate();
    }
}
