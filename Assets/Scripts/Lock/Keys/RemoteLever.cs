using UnityEngine;

public class RemoteLever : RemoteKeyBase
{
    [SerializeField]
    private Transform lever;

    [SerializeField]
    private float goalRot = -60f;

    protected override void Start()
    {
        base.Start();
        goalRot += lever.localRotation.eulerAngles.x;
    }

    protected override void Update()
    {
        base.Update();
        if (spent) return;

        Debug.Log($"{gameObject.name}: {lever.localRotation.eulerAngles.x}/{goalRot}");
        if (Mathf.Abs(lever.localRotation.eulerAngles.x - goalRot) < 1f)
        {
            Debug.Log($"{gameObject.name} Use");
            lever.localRotation = Quaternion.Euler(goalRot, 0f, 0f);
            OnUsed();
        }
    }
}