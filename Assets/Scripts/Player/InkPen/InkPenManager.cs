using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.UI.GridLayoutGroup;

public class InkPenManager : MonoBehaviour
{
    internal PlayerManager player = null;

    internal HandMapManager Map => player.Map();

    [SerializeField]
    internal Transform point = null;

    private Animator animator = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetHeld(InputDevice device)
    {
        heldDevice = device;
        SetHeld(true);
    }

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


    private InputDevice heldDevice;

    private bool lastTrigger = false;

    private void Update()
    {
        if (!Held)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0f) gameObject.SetActive(false);
            return;
        }

        if (!heldDevice.isValid) return;
        if (heldDevice.TryGetFeatureValue(CommonUsages.trigger, out var triggerValue))
        {
            if (triggerValue > 0.9f)
            {
                if (!lastTrigger) animator.SetBool("Out", true);
                Map.RequestPenDraw(point);
                lastTrigger = true;
            }
            else
            {
                if (lastTrigger) animator.SetBool("Out", false);
                lastTrigger = false;
                Map.PausePenDraw();
            }
        }

    }
}
