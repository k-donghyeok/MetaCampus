using UnityEngine;

/// <summary>
/// 에디터에서 문 열림 방향 표시
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(DoorLock))]
public class DebugDoorVisual : MonoBehaviour
{
    [SerializeField]
    private bool show = true;

#if UNITY_EDITOR
    private void Update()
    {
        if (!show) return;
        var door = GetComponent<DoorLock>();
        if (!door) return;
        const float S = 0.9f, A = 0.7f;
        var c = LockManager.GetColor(door.LockColorID);
        var O = transform.position;
        var B = door.Clockwise ? -transform.forward : transform.forward;

        Debug.DrawLine(O + B * S, O, c);
        Debug.DrawLine(O + B * S, O + transform.right * A + B * A, c);
        Debug.DrawLine(O + transform.right * A + B * A, O + transform.right * S, c);
        Debug.DrawLine(O + transform.right * A + B * A, O, c);
        Debug.DrawLine(O + B * S, O + B * S + transform.up * 2f, c);
        O += transform.up * 2f;
        Debug.DrawLine(O + B * S, O, c);
        Debug.DrawLine(O + B * S, O + transform.right * A + B * A, c);
        Debug.DrawLine(O + transform.right * A + B * A, O + transform.right * S, c);
        Debug.DrawLine(O + transform.right * A + B * A, O, c);
    }
#endif
}
