using System;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(GrabActionHandler))]
public class HandMapExpand : MonoBehaviour
{
    [SerializeField]
    private HandMapManager map = null;

    [SerializeField, Range(0f, 5f)]
    private float expandThreshold = 1f;
    [SerializeField, Range(0f, 5f)]
    private float retractSpeed = 1f;

    private GrabActionHandler grabActionHandler = null;

    private XRDirectInteractor[] DirectInteractors => grabActionHandler.directInteractors;

    private void Start()
    {
        if (!map)
            throw new ArgumentNullException(nameof(map));
        grabActionHandler = GetComponent<GrabActionHandler>();

        grabActionHandler.OnGrabbed += (left, device) =>
        {
            if (map.gameObject.activeSelf || grabActionHandler.GrabOccupied(left)) return;
            if (!device.TryGetFeatureValue(CommonUsages.devicePosition, out var pos)) return;
            if (pos.y < 0.8f) return; // 너무 낮음
            var posL = DirectInteractors[0].attachTransform.position; posL.y = 0f;
            var posR = DirectInteractors[1].attachTransform.position; posR.y = 0f;
            if (Vector3.Distance(posL, posR) > expandThreshold) return; //너무 멈  
            handFlags |= 1 << (left ? 0 : 1); // 잡기 플래그 켜기
        };
        grabActionHandler.OnGrabReleased += (left, device) =>
        {
            handFlags &= ~(1 << (left ? 0 : 1)); // 잡기 플래그 끄기
        };
    }

    private int handFlags = 0;

    private void Update()
    {
        if (handFlags == 3) // 양손을 쥐고 있는 경우, 지도를 펼칠 준비가 됨
        {
            if (map.gameObject.activeSelf) return; // 이미 들고 있음
            var posL = DirectInteractors[0].attachTransform.position; posL.y = 0f;
            var posR = DirectInteractors[1].attachTransform.position; posR.y = 0f;
            if (Vector3.Distance(posL, posR) > expandThreshold) EnableMap(); // 수평으로 충분히 멀면, 지도를 펼치기
        }
        else if (map.gameObject.activeSelf)
        {
            if (map.layDown) return; // 놓여진 상태에서는 접지 않음

            // 지도를 손에서 떨어뜨리기
            if ((handFlags & (1 << 0)) == 0)
            {
                map.handleLeft.SetParent(map.transform);
                grabActionHandler.RequestHandAnimation(true, HandAnimator.SpecialAnimation.None);
            }
            if ((handFlags & (1 << 1)) == 0)
            {
                map.handleRight.SetParent(map.transform);
                grabActionHandler.RequestHandAnimation(false, HandAnimator.SpecialAnimation.None);
            }

            if (handFlags > 0)
            { // 한 손을 놓고 있으면 놓은 쪽부터 지도가 말려들어감
                var leftToRight = map.handleRight.position - map.handleLeft.position;
                leftToRight = Vector3.ClampMagnitude(leftToRight, retractSpeed * Time.deltaTime);
                if ((handFlags & (1 << 0)) > 0) map.handleRight.Translate(-leftToRight, Space.World);
                else map.handleLeft.Translate(leftToRight, Space.World);
            }
            else
            { // 양 손을 놓고 있으면 가운데로 지도가 말려들어감
                var center = Vector3.Lerp(map.handleLeft.position, map.handleRight.position, 0.5f);
                map.handleLeft.Translate(Vector3.ClampMagnitude(center - map.handleLeft.position, retractSpeed * 0.5f * Time.deltaTime), Space.World);
                map.handleRight.Translate(Vector3.ClampMagnitude(center - map.handleRight.position, retractSpeed * 0.5f * Time.deltaTime), Space.World);
            }

            if (!map.dissappearing && Vector3.Distance(map.handleLeft.position, map.handleRight.position) < expandThreshold)
            {
                map.dissappearing = true; // 지도가 어느정도 말려들어가면 사라지는 애니메이션 실행
                for (int i = 0; i < 2; ++i)
                    grabActionHandler.RequestHandAnimation(i, HandAnimator.SpecialAnimation.None);
            }
        }
    }

    private void EnableMap()
    {
        map.gameObject.SetActive(true);

        map.handleLeft.transform.SetParent(DirectInteractors[0].attachTransform);
        map.handleLeft.transform.SetLocalPositionAndRotation(Vector3.right * 0.05f, Quaternion.Euler(20f, 0f, 0f));
        map.handleRight.transform.SetParent(DirectInteractors[1].attachTransform);
        map.handleRight.transform.SetLocalPositionAndRotation(Vector3.left * 0.05f, Quaternion.Euler(20f, 0f, 0f));

        map.CreateToggleEffect();
        map.SetHeld(true);
        for (int i = 0; i < 2; ++i)
            grabActionHandler.RequestHandAnimation(i, HandAnimator.SpecialAnimation.GripMap);
    }


}
