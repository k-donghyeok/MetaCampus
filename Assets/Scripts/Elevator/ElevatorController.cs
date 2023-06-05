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

    [Header("Prefabs")]
    [SerializeField]
    private GameObject floorPrefab = null;
    [SerializeField]
    private GameObject buttonPrefab = null;

    public delegate void StatusUpdateHandler(string status);

    public StatusUpdateHandler OnStatusUpdate = null;

    private ElevatorFloor[] elevFloors;
    private ElevatorButtonFloor[] elevButtons;

    private void Start()
    {
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
        CurStatusIndex = 0;
    }

    private bool isMoving = false;

    private int CurStatusIndex
    {
        get => curStatusIndex;
        set
        {
            if (curStatusIndex == value) return;
            curStatusIndex = value;
            OnStatusUpdate?.Invoke(floors[curStatusIndex].name);
        }
    }
    private int curStatusIndex = -1;

    private void CalculateStatusIndex()
    {
        float curHeight = chamberRbody.transform.localPosition.y;

        if (CurStatusIndex > 0 && curHeight < floors[CurStatusIndex].height)
        {
            float mid = Mathf.Lerp(floors[CurStatusIndex - 1].height, floors[CurStatusIndex].height, 0.5f);
            if (curHeight < mid) --CurStatusIndex;
        }
        else if (CurStatusIndex < floors.Length - 2 && curHeight > floors[CurStatusIndex].height)
        {
            float mid = Mathf.Lerp(floors[CurStatusIndex].height, floors[CurStatusIndex + 1].height, 0.5f);
            if (curHeight > mid) ++CurStatusIndex;
        }
    }

    public void RequestMoveToFloor(int index, bool exterior)
    {
        if (CurStatusIndex == index && !isMoving)
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

        // ���� ���� ������ �̵� ����
        isMoving = true;
        yield return null;

        // �̵� ��
        isMoving = false;
        RequestOpenDoor();
    }

    private void Update()
    {
        if (isMoving) return;

    }

    /// <summary>
    /// ���� �����ִ� ����
    /// </summary>
    private float doorOpen = 0f;
    /// <summary>
    /// ���� �󸶳� ���� �����־���ϴ���
    /// </summary>
    private float doorOpenHang = 0f;

    public void RequestOpenDoor()
    {
        if (isMoving) return;
        // �� ������ �ִϸ��̼� ���
    }

    public void RequestCloseDoor()
    {
        // ���� ������ �ִϸ��̼� ���
    }


}
