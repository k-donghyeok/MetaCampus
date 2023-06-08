using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [Serializable]
    private struct FloorData
    {
        /// <summary>
        /// ��ư�� ǥ�õǴ� �̸�
        /// </summary>
        public string name;
        /// <summary>
        /// ���������� ��Ʈ�ѷ� ���� ���� (m ����)
        /// </summary>
        public float height;
    }

    [SerializeField]
    private FloorData[] floors = new FloorData[1];

    [Header("Chamber")]
    [SerializeField]
    private RectTransform chamberPanel = null;
    [SerializeField]
    private TMP_Text chamberTxtStatus = null;
    [SerializeField]
    private Rigidbody chamberRbody = null;
    [SerializeField]
    private Collider chamberTrigger = null;
    [SerializeField]
    private Animator chamberAnim = null;
    [SerializeField, Range(1f, 4f)]
    private float doorCloseTime = 2f;
    [SerializeField, Range(1f, 20f)]
    private float doorOpenWaitTime = 8f;
    [SerializeField, Range(0.1f, 4.0f)]
    private float chamberSpeed = 1f;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject floorPrefab = null;
    [SerializeField]
    private GameObject buttonPrefab = null;

    public delegate void StatusUpdateHandler(string status);

    public StatusUpdateHandler OnStatusUpdate = null;

    private ElevatorFloor[] elevFloors;
    private ElevatorButtonFloor[] elevButtons;
    private float doorCloseSpeed = 1f;
    private const float accelerationTime = 1f;
    private float currentSpeed;

    private void Start()
    {
        doorCloseSpeed = 1f / doorCloseTime;

        List<ElevatorFloor> elevFloors = new();
        List<ElevatorButtonFloor> elevButtons = new();
        // floors�� ������ŭ
        for (int i = 0; i < floors.Length; ++i)
        {
            // floorPrefab�� �ν��Ͻ��� �����
            {
                var go = Instantiate(floorPrefab, transform);
                go.name = $"Floor {floors[i].name}";
                go.transform.localPosition = new Vector3(0f, floors[i].height, 0f);
                var floor = go.GetComponent<ElevatorFloor>();
                floor.Initiate(this, i, floors[i].name);
                elevFloors.Add(floor);
            }

            // chamber ���ο� ������ ���� ��ư�� �����
            {
                var go = Instantiate(buttonPrefab, chamberPanel);
                go.name = $"Button {floors[i].name}";
                go.transform.localPosition = new Vector2(0f, 400f - 110f * (floors.Length - 1 - i));
                var button = go.GetComponent<ElevatorButtonFloor>();
                button.Initiate(this, i, floors[i].name);
                elevButtons.Add(button);
            }
        }
        this.elevFloors = elevFloors.ToArray();
        this.elevButtons = elevButtons.ToArray();

        // ���������� ���°� ����ɶ� ĭ ���� ���� �ؽ�Ʈ�� �ٲ۴�
        OnStatusUpdate += (status) => chamberTxtStatus.text = status;

        // ù ��° ������ ���������͸� ��ġ
        chamberRbody.transform.localPosition = new Vector3(0f, floors[0].height, 0f);
        CurIndex = 0;
    }

    private bool isMoving = false;

    private int CurIndex
    {
        get => curIndex;
        set
        {
            if (curIndex == value) return;
            curIndex = value;
            OnStatusUpdate?.Invoke(floors[curIndex].name);
        }
    }
    private int curIndex = -1;

    private void CalculateCurIndex()
    {
        float curHeight = chamberRbody.transform.localPosition.y;

        if (CurIndex > 0 && curHeight < floors[CurIndex].height)
        {
            float mid = Mathf.Lerp(floors[CurIndex - 1].height, floors[CurIndex].height, 0.5f);
            if (curHeight < mid) --CurIndex;
        }
        else if (CurIndex < floors.Length - 1 && curHeight > floors[CurIndex].height)
        {
            float mid = Mathf.Lerp(floors[CurIndex].height, floors[CurIndex + 1].height, 0.5f);
            if (curHeight > mid) ++CurIndex;
        }
    }

    public void RequestMoveToFloor(int index, bool exterior)
    {
        if (CurIndex == index && !isMoving)
        {
            if (exterior) RequestOpenDoor();
            return;
        }
        if (MoveCoroutine != null) StopCoroutine(MoveCoroutine);
        MoveCoroutine = StartCoroutine(MoveToFloor(index));
    }

    private Coroutine MoveCoroutine = null;

    private IEnumerator MoveToFloor(int index)
    {
        // ���� ���� ������ ���
        while (doorOpen > 0f)
            yield return new WaitForSeconds(0.5f);
        // ���� ���� ������ �̵� ����
        isMoving = true;
        chamberRbody.isKinematic = false;

        // ��ǥ ����
        float goalPos = floors[index].height;
        // ���̱��� ���� �Ÿ� offset
        float distance = goalPos - chamberRbody.transform.localPosition.y;

        // ���������� �ȿ� �ִ� Rigidbody�� ������
        Collider[] carriedColliders = Physics.OverlapBox(chamberTrigger.bounds.center, chamberTrigger.bounds.extents, chamberTrigger.transform.rotation, LayerMask.GetMask("Player") | LayerMask.GetMask("Item"));
        List<Rigidbody> carriedRbodies = new();
        foreach (var c in carriedColliders)
            if (c.TryGetComponent<Rigidbody>(out var rigidbody)) carriedRbodies.Add(rigidbody);

        while (Mathf.Abs(distance) > 0.001f)
        {
            // ���� �Ÿ�
            distance = goalPos - chamberRbody.transform.localPosition.y;
            // �� ���������ӿ� �̵��ϴ� �Ÿ�
            float moveDistance = currentSpeed * Time.fixedDeltaTime;
            // ���� ���� �Ÿ�
            float decelerationDistance = (currentSpeed * currentSpeed) / (2f * chamberSpeed);

            if (Mathf.Abs(distance) <= decelerationDistance && currentSpeed != 0f) // ���� �Ÿ� ���̸� ����
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, chamberSpeed * Time.fixedDeltaTime);
            else if (Mathf.Abs(moveDistance) < Mathf.Abs(distance)) // ���� �Ÿ��� �ִٸ� ���� Ȥ�� ����
                currentSpeed = Mathf.MoveTowards(currentSpeed, Mathf.Sign(distance) * chamberSpeed, chamberSpeed * Time.fixedDeltaTime);
            else currentSpeed = 0f;

            // ���������� �� ���� Rigidbody�� �ӵ� ����
            chamberRbody.velocity = new Vector3(0f, currentSpeed, 0f);
            foreach (var rb in carriedRbodies)
                rb.velocity = new Vector3(rb.velocity.x, currentSpeed, rb.velocity.z);

            yield return new WaitForFixedUpdate();
            CalculateCurIndex(); // �ǽð� ���� ���
        }

        // �̵� ��: ���������͸� ��ǥ ��ġ �� �ӵ��� ����
        isMoving = false;
        currentSpeed = 0f;
        chamberRbody.velocity = Vector3.zero;
        chamberRbody.transform.localPosition = new Vector3(0f, goalPos, 0f);
        chamberRbody.isKinematic = true; // ������������ ���� ����

        // �ٸ� ������ ���� �� ���� �̺�Ʈ (�� ����) ����
        CurIndex = index;
        RequestOpenDoor();
    }

    private void Update()
    {
        if (isMoving) return;

        // doorOpen ���
        if (doorOpenHang > 0f)
        {
            if (doorOpen < 1f) // ���� �� ���� ���¸� ���� ��
            {
                doorOpen += Time.deltaTime * doorCloseSpeed;
                if (doorOpen > 1f) doorOpen = 1f;
            }
            else  // ���� ���� ������ ���ð� ����
            {
                doorOpenHang -= Time.deltaTime;
                if (doorOpenHang < 0f) doorOpenHang = 0f;
            }

        }
        else if (doorOpen > 0f) // ���� ����
        {
            doorOpen -= Time.deltaTime * doorCloseSpeed;
            if (doorOpen < 0f) doorOpen = 0f;
        }

        chamberAnim.SetFloat("open", doorOpen);
        elevFloors[CurIndex].UpdateAnim(doorOpen);
    }

    /// <summary>
    /// ���� �����ִ� ����. 0: ����, 1: ����
    /// </summary>
    private float doorOpen = 0f;
    /// <summary>
    /// ���� �󸶳� ���� �����־���ϴ���
    /// </summary>
    private float doorOpenHang = 0f;

    public void RequestOpenDoor()
    {
        if (isMoving) return;
        doorOpenHang = doorOpenWaitTime;
    }

    public void RequestCloseDoor()
    {
        doorOpenHang = 0f;
    }

    public void OnDoorCollision()
    {
        if (isMoving) return;
        if (doorOpenHang == 0f && doorOpen > 0f) // ���� ������ �ִ� ��
        {
            RequestOpenDoor();
        }
    }
}
