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
    private void Start()
    {
        if (Application.isPlaying) Destroy(this); 
    }

#if UNITY_EDITOR
    private void Update()
    {
        var key = GetComponent<DoorKey>();
        if (!key) return;
        const float S = 0.5f;
        var c = LockManager.GetColor(key.LockColorID);
        Debug.DrawLine(transform.position + Vector3.forward * S, transform.position + Vector3.right * S, c);
        Debug.DrawLine(transform.position + Vector3.right * S, transform.position + Vector3.back * S, c);
        Debug.DrawLine(transform.position + Vector3.back * S, transform.position + Vector3.left * S, c);
        Debug.DrawLine(transform.position + Vector3.left * S, transform.position + Vector3.forward * S, c);
        Debug.DrawLine(transform.position + Vector3.forward * S, transform.position + Vector3.up * 2f, c);
        Debug.DrawLine(transform.position + Vector3.right * S, transform.position + Vector3.up * 2f, c);
        Debug.DrawLine(transform.position + Vector3.back * S, transform.position + Vector3.up * 2f, c);
        Debug.DrawLine(transform.position + Vector3.left * S, transform.position + Vector3.up * 2f, c);
    }
#endif
}
