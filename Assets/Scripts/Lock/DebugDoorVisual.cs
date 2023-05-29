using UnityEngine;

/// <summary>
/// 에디터에서 문 열림 방향 표시
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(DoorLock))]
public class DebugDoorVisual : MonoBehaviour
{
#if UNITY_EDITOR
    private void Update()
    {
        var door = GetComponent<DoorLock>();
        if (!door) return;
        const float S = 0.9f, A = 0.7f;
        var c = LockManager.GetColor(door.LockColorID);
        var O = transform.position;
        var B = door.Clockwise ? Vector3.back : Vector3.forward;

        Debug.DrawLine(O + B * S, O, c);
        Debug.DrawLine(O + B * S, O + Vector3.right * A + B * A, c);
        Debug.DrawLine(O + Vector3.right * A + B * A, O + Vector3.right * S, c);
        Debug.DrawLine(O + Vector3.right * A + B * A, O, c);
        Debug.DrawLine(O + B * S, O + B * S + Vector3.up * 2f, c);
        O += Vector3.up * 2f;
        Debug.DrawLine(O + B * S, O, c);
        Debug.DrawLine(O + B * S, O + Vector3.right * A + B * A, c);
        Debug.DrawLine(O + Vector3.right * A + B * A, O + Vector3.right * S, c);
        Debug.DrawLine(O + Vector3.right * A + B * A, O, c);
    }
#endif
}
