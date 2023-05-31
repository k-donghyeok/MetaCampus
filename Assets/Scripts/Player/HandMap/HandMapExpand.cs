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
            if (pos.y < 0.8f) return; // �ʹ� ����
            var posL = DirectInteractors[0].attachTransform.position; posL.y = 0f;
            var posR = DirectInteractors[1].attachTransform.position; posR.y = 0f;
            if (Vector3.Distance(posL, posR) > expandThreshold) return; //�ʹ� ��  
            handFlags |= 1 << (left ? 0 : 1); // ��� �÷��� �ѱ�
        };
        grabActionHandler.OnGrabReleased += (left, device) =>
        {
            handFlags &= ~(1 << (left ? 0 : 1)); // ��� �÷��� ����
        };
    }

    private int handFlags = 0;

    private void Update()
    {
        if (handFlags == 3) // ����� ��� �ִ� ���, ������ ��ĥ �غ� ��
        {
            if (map.gameObject.activeSelf) return; // �̹� ��� ����
            var posL = DirectInteractors[0].attachTransform.position; posL.y = 0f;
            var posR = DirectInteractors[1].attachTransform.position; posR.y = 0f;
            if (Vector3.Distance(posL, posR) > expandThreshold) EnableMap(); // �������� ����� �ָ�, ������ ��ġ��
        }
        else if (map.gameObject.activeSelf)
        {
            if (map.layDown) return; // ������ ���¿����� ���� ����

            // ������ �տ��� ����߸���
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
            { // �� ���� ���� ������ ���� �ʺ��� ������ ������
                var leftToRight = map.handleRight.position - map.handleLeft.position;
                leftToRight = Vector3.ClampMagnitude(leftToRight, retractSpeed * Time.deltaTime);
                if ((handFlags & (1 << 0)) > 0) map.handleRight.Translate(-leftToRight, Space.World);
                else map.handleLeft.Translate(leftToRight, Space.World);
            }
            else
            { // �� ���� ���� ������ ����� ������ ������
                var center = Vector3.Lerp(map.handleLeft.position, map.handleRight.position, 0.5f);
                map.handleLeft.Translate(Vector3.ClampMagnitude(center - map.handleLeft.position, retractSpeed * 0.5f * Time.deltaTime), Space.World);
                map.handleRight.Translate(Vector3.ClampMagnitude(center - map.handleRight.position, retractSpeed * 0.5f * Time.deltaTime), Space.World);
            }

            if (!map.dissappearing && Vector3.Distance(map.handleLeft.position, map.handleRight.position) < expandThreshold)
            {
                map.dissappearing = true; // ������ ������� �������� ������� �ִϸ��̼� ����
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
