using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PasswordKey : DoorKey
{
    [SerializeField]
    private TMP_Text[] texts = new TMP_Text[0];

    private void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.Password;
    }

    protected override void Start()
    {
        base.Start();
        StageManager.Instance().OnStageLoad += (stage) => DisplayPassword(stage);
    }

    private void DisplayPassword(StageManager stage)
    {
        var pw = stage.Lock.GetPassword(LockColorID);
        //Debug.Log($"{gameObject.name}({LockColorID}) 비밀번호: [{pw}]");
        foreach (var text in texts) text.SetText(pw.ToString());
    }

    protected override void Update()
    {
        if (!Held) FloatUpdate();
    }

    protected override void FloatUpdate()
    {
        if (!groundModel) return;
        float angleDeg = (Time.time * 60f) % 360f;

        groundModel.localPosition = new Vector3(groundModel.localPosition.x,
            Mathf.Sin(Mathf.Deg2Rad * angleDeg) * 0.05f,
            groundModel.localPosition.z);
    }

    public override void OnTrigger()
    {
    }
}
