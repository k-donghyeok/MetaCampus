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
        Debug.DrawLine(O + transform.forward * S, O + transform.right * S, c);
        Debug.DrawLine(O + transform.right * S, O - transform.forward * S, c);
        Debug.DrawLine(O - transform.forward * S, O - transform.right * S, c);
        Debug.DrawLine(O - transform.right * S, O + transform.forward * S, c);
        Debug.DrawLine(O + transform.forward * S, O + transform.up * 2f, c);
        Debug.DrawLine(O + transform.right * S, O + transform.up * 2f, c);
        Debug.DrawLine(O - transform.forward * S, O + transform.up * 2f, c);
        Debug.DrawLine(O - transform.right * S, O + transform.up * 2f, c);
    }
#endif
}
