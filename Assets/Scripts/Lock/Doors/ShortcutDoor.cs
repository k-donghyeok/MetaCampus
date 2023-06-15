using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShortcutDoor : DoorLock
{
    [SerializeField]
    private Transform handles = null;

    [SerializeField]
    private AudioClip openSound;
    private AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        if (!Clockwise) handles.transform.Rotate(0f, 180f, 0f, Space.Self);
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void TryOpen(XRBaseInteractable self)
    {
        var dir = PlayerManager.InstanceOrigin().position - transform.position;
        var dot = Vector3.Dot(dir, Clockwise ? transform.forward : -transform.forward);
        Debug.Log($"{gameObject.name} TryOpen: {dot}");
        if (dot < 0f) return; // Not Front

        IsUnlocked = true;
        PlayOpenAnimation();
        PlayOpenSound();
        self.enabled = false;
    }

    private void PlayOpenSound()
    {
        if (openSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(openSound);
        }
    }
}