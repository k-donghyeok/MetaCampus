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
    protected Transform groundModel = null;
    [SerializeField]
    protected Transform heldModel = null;

    protected virtual void Start()
    {
        LockManager.DyeRenderers(LockColorID, dyeRenderers);
        groundModel.gameObject.SetActive(true);
        heldModel.gameObject.SetActive(false);

        FloatUpdate();
    }

    protected virtual void Update()
    {
        if (!Held) FloatUpdate();
    }

    protected virtual void FloatUpdate()
    {
        if (!groundModel) return;
        float angleDeg = (60f * Time.time) % 360f;

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

    public virtual void OnTrigger()
    {
        if (!Physics.Raycast(transform.position, transform.forward, out var info, 0.2f, 1 << 7)) return;
        if (!info.transform.TryGetComponent<DoorLock>(out var door)) return;
        Debug.Log($"열쇠 {gameObject.name} > {info.transform.gameObject.name} 개방 시도");
        if (door.TryUnlock(this)) OnUsed();
    }

    protected virtual void OnUsed()
    {

    }
}
