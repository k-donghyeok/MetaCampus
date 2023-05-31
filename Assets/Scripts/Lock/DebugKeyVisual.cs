using UnityEngine;

/// <summary>
/// 에디터에서 열쇠 주위로 마름모 표시
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(DoorKey))]
public class DebugKeyVisual : MonoBehaviour
{
    //private void Start()
    //{
    //    if (Application.isPlaying) Destroy(this); 
    //}

#if UNITY_EDITOR
    private void Update()
    {
        var key = GetComponent<DoorKey>();
        if (!key) return;
        const float S = 0.5f;
        var c = LockManager.GetColor(key.LockColorID);
        var O = transform.position;
        Debug.DrawLine(O + Vector3.forward * S, O + Vector3.right * S, c);
        Debug.DrawLine(O + Vector3.right * S, O + Vector3.back * S, c);
        Debug.DrawLine(O + Vector3.back * S, O + Vector3.left * S, c);
        Debug.DrawLine(O + Vector3.left * S, O + Vector3.forward * S, c);
        Debug.DrawLine(O + Vector3.forward * S, O + Vector3.up * 2f, c);
        Debug.DrawLine(O + Vector3.right * S, O + Vector3.up * 2f, c);
        Debug.DrawLine(O + Vector3.back * S, O + Vector3.up * 2f, c);
        Debug.DrawLine(O + Vector3.left * S, O + Vector3.up * 2f, c);
    }
#endif
}
