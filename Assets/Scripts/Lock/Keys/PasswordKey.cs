using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordKey : DoorKey
{
    private void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.Password;
    }

    protected override void Start()
    {
        base.Start();
        DisplayPassword();
    }

    private void DisplayPassword()
    {
        var pw = StageManager.Instance().Lock.GetPassword(LockColorID);
    }

    protected override void Update()
    {
        if (!Held) FloatUpdate();
    }

    private float angleDeg;

    private void FloatUpdate()
    {
        if (!groundModel) return;
        angleDeg = (angleDeg + 60f * Time.deltaTime) % 360f;

        groundModel.localPosition = new Vector3(groundModel.localPosition.x,
            Mathf.Sin(Mathf.Deg2Rad * angleDeg) * 0.05f,
            groundModel.localPosition.z);
    }
}
