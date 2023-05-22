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
            if ((handFlags & (1 << 0)) == 0) map.handleLeft.transform.SetParent(map.transform);
            if ((handFlags & (1 << 1)) == 0) map.handleRight.transform.SetParent(map.transform);

            var leftToRight = map.handleRight.position - map.handleLeft.position;
            leftToRight = Vector3.ClampMagnitude(leftToRight, retractSpeed * Time.deltaTime);
            if (handFlags > 0)
            { // �� ���� ���� ������ ���� �ʺ��� ������ ������
                if ((handFlags & (1 << 0)) > 0) map.handleRight.transform.Translate(-leftToRight);
                else map.handleLeft.transform.Translate(leftToRight);
            }
            else
            { // �� ���� ���� ������ ����� ������ ������
                leftToRight *= 0.5f;
                map.handleRight.transform.Translate(-leftToRight);
                map.handleLeft.transform.Translate(leftToRight);
            }

            if (!map.dissappearing && Vector3.Distance(map.handleLeft.position, map.handleRight.position) < expandThreshold)
                map.dissappearing = true; // ������ ������� �������� ������� �ִϸ��̼� ����
        }
    }

    private void EnableMap()
    {
        map.gameObject.SetActive(true);

        map.handleLeft.transform.SetParent(DirectInteractors[0].attachTransform);
        map.handleLeft.transform.SetLocalPositionAndRotation(Vector3.right * 0.1f, Quaternion.Euler(20f, 0f, 0f));
        map.handleRight.transform.SetParent(DirectInteractors[1].attachTransform);
        map.handleRight.transform.SetLocalPositionAndRotation(Vector3.left * 0.1f, Quaternion.Euler(20f, 0f, 0f));

        map.CreateToggleEffect();
        map.SetLaydown(true);
    }


}
