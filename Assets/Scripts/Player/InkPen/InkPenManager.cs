using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkPenManager : MonoBehaviour
{
    internal PlayerManager player = null;

    internal HandMapManager Map => player.Map();

    [SerializeField]
    internal Transform point = null;

    public void SetHeld(bool held)
    {
        if (Held == held) return;
        Held = held;
        hideTimer = 1f;
        if (held) Map.LaydownMap();
        else Map.RequestFoldLaydownMap();
    }

    /// <summary>
    /// 이것이 플레이어 손에 들려있는지 여부
    /// </summary>
    public bool Held { get; private set; }

    private float hideTimer = 1f;

    private void Update()
    {
        if (!Held)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0f) gameObject.SetActive(false);
            return;
        }

        Map.RequestPenDraw(transform, point);
    }
}
