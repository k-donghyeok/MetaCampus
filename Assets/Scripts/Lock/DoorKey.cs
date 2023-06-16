using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
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

    [SerializeField]
    private AudioClip heldSound;
    private AudioSource audioSource;

    protected float sqrdInteractionDistance = 0.36f; // 0.6f

    protected virtual void Start()
    {
        LockManager.DyeRenderers(LockColorID, dyeRenderers);
        if (groundModel) groundModel.gameObject.SetActive(true);
        if (heldModel) heldModel.gameObject.SetActive(false);

        FloatUpdate();

        audioSource = GetComponent<AudioSource>();
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
            if (TryGetComponent<CapsuleCollider>(out var itemCollider)) itemCollider.enabled = !held;
            if (groundModel) groundModel.gameObject.SetActive(!held);
            if (heldModel) heldModel.gameObject.SetActive(held);
        }
    }
    private bool held = false;

    public virtual void OnHeld()
    {
        var player = PlayerManager.InstanceOrigin();
        if (player)
        {
            if (Mathf.Pow(player.position.x - transform.position.x, 2f) + Mathf.Pow(player.position.z - transform.position.z, 2f) > sqrdInteractionDistance
                || Physics.Raycast(player.position, transform.position + transform.up, 1f, LayerMask.GetMask("Obstacle")))
            {
                var i = GetComponentInChildren<XRBaseInteractable>();
                if (i) { i.enabled = false; i.enabled = true; }
                return;
            }
        }
        Held = true;

        PlayHeldSound();
    }

    public virtual void OnHeldReleased()
    {
        Held = false;
        if (groundModel) transform.rotation = Quaternion.identity;
        FloatUpdate();
    }

    public virtual void OnTrigger()
    {
        if (!Physics.Raycast(transform.position, transform.forward, out var info, 0.3f, 1 << 7)) return;
        var door = info.transform.GetComponentInParent<DoorLock>();
        if (!door) return;
        Debug.Log($"열쇠 {gameObject.name} > {door.gameObject.name} 개방 시도");
        if (door.TryUnlock(this)) OnUsed();
    }

    protected virtual void OnUsed()
    {

    }

    private void PlayHeldSound()
    {
        if (heldSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(heldSound);
        }
    }
}
