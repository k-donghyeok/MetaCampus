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
        /// 버튼에 표시되는 이름
        /// </summary>
        public string name;
        /// <summary>
        /// 엘리베이터 컨트롤러 기준 높이 (m 유닛)
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

    private void Start()
    {
        doorCloseSpeed = 1f / doorCloseTime;

        List<ElevatorFloor> elevFloors = new();
        List<ElevatorButtonFloor> elevButtons = new();
        // floors의 개수만큼
        for (int i = 0; i < floors.Length; ++i)
        {
            // floorPrefab의 인스턴스를 만든다
            {
                var go = Instantiate(floorPrefab, transform);
                go.name = $"Floor {floors[i].name}";
                go.transform.localPosition = new Vector3(0f, floors[i].height, 0f);
                var floor = go.GetComponent<ElevatorFloor>();
                floor.Initiate(this, i, floors[i].name);
                elevFloors.Add(floor);
            }

            // chamber 내부에 층별로 가는 버튼을 만든다
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

        // 엘리베이터 상태가 변경될때 칸 안의 상태 텍스트를 바꾼다
        OnStatusUpdate += (status) => chamberTxtStatus.text = status;

        // 첫 번째 층으로 엘리베이터를 위치
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
        // 문이 열려 있으면 대기
        while (doorOpen > 0f)
            yield return new WaitForSeconds(0.5f);
        // 문이 닫혀 있으면 이동 시작
        isMoving = true;

        float goalPos = floors[index].height;

        while (Mathf.Abs(chamberRbody.transform.localPosition.y - goalPos) > 0.001f // 원하는 위치에 도달
            || chamberRbody.velocity.magnitude > 0.01f) // 속도 멈춤
        {
            // TODO: FIX
            var vel = chamberRbody.velocity;
            float dir = goalPos - chamberRbody.transform.localPosition.y;
            float acc = Mathf.Sign(dir) * Time.fixedDeltaTime * chamberSpeed;
            if (Mathf.Abs(dir) < vel.y * vel.y / (chamberSpeed * 2f))
            {
                if (Mathf.Abs(vel.y) > Mathf.Abs(acc)) vel.y -= acc;
                else vel.y = 0f;
            }
            else
            {
                vel.y = Mathf.Clamp(vel.y + acc, -chamberSpeed, chamberSpeed);
            }
            chamberRbody.velocity = vel;

            yield return new WaitForFixedUpdate();
            CalculateCurIndex();
        }

        // 이동 끝
        isMoving = false;
        CurIndex = index;
        RequestOpenDoor();
    }

    private void Update()
    {
        if (isMoving) return;

        // doorOpen 계산
        if (doorOpenHang > 0f)
        {
            if (doorOpen < 1f) // 문이 안 열린 상태면 문을 염
            {
                doorOpen += Time.deltaTime * doorCloseSpeed;
                if (doorOpen > 1f) doorOpen = 1f;
            }
            else  // 문이 열린 상태의 대기시간 감소
            {
                doorOpenHang -= Time.deltaTime;
                if (doorOpenHang < 0f) doorOpenHang = 0f;
            }

        }
        else if (doorOpen > 0f) // 문이 닫힘
        {
            doorOpen -= Time.deltaTime * doorCloseSpeed;
            if (doorOpen < 0f) doorOpen = 0f;
        }
        
        chamberAnim.SetFloat("open", doorOpen);
        elevFloors[CurIndex].UpdateAnim(doorOpen);
    }

    /// <summary>
    /// 문이 열려있는 정도. 0: 닫힘, 1: 열림
    /// </summary>
    private float doorOpen = 0f;
    /// <summary>
    /// 문이 얼마나 오래 열려있어야하는지
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
        if (doorOpenHang == 0f && doorOpen > 0f) // 문이 닫히고 있는 중
        {
            RequestOpenDoor();
        }
    }
}
