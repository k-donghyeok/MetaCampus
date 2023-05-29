using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 에디터에서 열쇠 주위로 마름모 표시
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(DoorKey))]
public class DebugKeyVisual : MonoBehaviour
{
#if UNITY_EDITOR
    private void Update()
    {
        var key = GetComponent<DoorKey>();
        if (!key) return;
        var c = LockManager.GetColor(key.LockColorID);
        Debug.DrawLine(transform.position + Vector3.forward, transform.position + Vector3.right, c);
        Debug.DrawLine(transform.position + Vector3.right, transform.position + Vector3.back, c);
        Debug.DrawLine(transform.position + Vector3.back, transform.position + Vector3.left, c);
        Debug.DrawLine(transform.position + Vector3.left, transform.position + Vector3.forward, c);
        Debug.DrawLine(transform.position + Vector3.forward, transform.position + Vector3.up * 2f, c);
        Debug.DrawLine(transform.position + Vector3.right, transform.position + Vector3.up * 2f, c);
        Debug.DrawLine(transform.position + Vector3.back, transform.position + Vector3.up * 2f, c);
        Debug.DrawLine(transform.position + Vector3.left, transform.position + Vector3.up * 2f, c);
    }
#endif
}
