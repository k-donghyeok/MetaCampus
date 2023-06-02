using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FreeDoor : DoorLock
{
    [SerializeField]
    private XRGrabInteractable interactable;
    [SerializeField]
    private HingeJoint joint;

    [SerializeField]
    private Rigidbody doorBody;

    protected override void Start()
    {
        base.Start();
        if (!Clockwise)
        {
            var limits = joint.limits;
            limits.min = -90f;
            limits.max = 0f;
            joint.limits = limits;
        }
        IsUnlocked = true;
    }

    private void Update()
    {
        if ((doorBody.transform.position - transform.position).magnitude > 2f
            || doorBody.velocity.magnitude > 8f) Reset();

        if (interactor == null) return;
        if (Vector3.Distance(interactor.transform.position, transform.position) > 2f)
        {
            interactable.enabled = false;
            OnGrabReleased();
            interactable.enabled = true;
        }
    }

    private void Reset()
    {
        doorBody.transform.localPosition = Vector3.zero;
        doorBody.transform.rotation = Quaternion.identity;
        doorBody.velocity = Vector3.zero;

        interactable.enabled = false;
        OnGrabReleased();
        interactable.enabled = true;
    }

    private IXRInteractor interactor = null;

    public void OnGrabbed()
    {
        interactor = interactable.firstInteractorSelecting;
    }

    public void OnGrabReleased()
    {
        interactor = null;
    }
}