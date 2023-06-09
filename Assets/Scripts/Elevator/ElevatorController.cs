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

    [SerializeField]
    private Transform shaftHolder = null;

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
    [SerializeField]
    private GameObject shaftPrefab = null;
    [SerializeField]
    private GameObject horzbarPrefab = null;

    public delegate void StatusUpdateHandler(string status);

    public StatusUpdateHandler OnStatusUpdate = null;

    private ElevatorFloor[] elevFloors;
    private ElevatorButtonFloor[] elevButtons;
    private float doorCloseSpeed = 1f;
    private const float accelerationTime = 1f;
    private float currentSpeed;

    [Header("Audio")]
    [SerializeField]
    private AudioClip doorOpenSound;
    [SerializeField]
    private AudioClip doorCloseSound;
    [SerializeField]
    private AudioClip floorArrivalSound;
    [SerializeField]
    private AudioClip doorOpenButtonSound;
    [SerializeField]
    private AudioClip doorCloseButtonSound;
    [SerializeField]
    private AudioClip doorAutoCloseSound;


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

            if (i > 0)
            {
                float offset = floors[i].height - floors[i - 1].height;
                if (offset > 3f)
                {
                    var shaft = Instantiate(shaftPrefab, shaftHolder);
                    shaft.name = $"Shaft {floors[i].name}";
                    shaft.transform.localPosition = new Vector3(0f, floors[i - 1].height + 3f, 0f);
                    shaft.transform.localScale = new Vector3(1f, offset - 3f, 1f);
                    for (float b = floors[i - 1].height + 3.5f; b < floors[i].height - 0.2f; ++b)
                    {
                        var beam = Instantiate(horzbarPrefab, shaftHolder);
                        beam.name = $"Beam {floors[i - 1].name} - {Mathf.FloorToInt(b)}";
                        beam.transform.localPosition = new Vector3(0f, b, 0f);
                    }
                }
            }
            else
            {
                var shaft = Instantiate(shaftPrefab, shaftHolder);
                shaft.name = "Shaft Root";
                shaft.transform.localPosition = new Vector3(0f, -1f, 0f);
                var beam = Instantiate(horzbarPrefab, shaftHolder);
                beam.name = "Beam Root";
                beam.transform.localPosition = new Vector3(0f, -0.8f, 0f);
            }
            if (i == floors.Length - 1)
            {
                var shaft = Instantiate(shaftPrefab, shaftHolder);
                shaft.name = "Shaft Cap";
                shaft.transform.localPosition = new Vector3(0f, floors[i].height + 3f, 0f);
                var beam = Instantiate(horzbarPrefab, shaftHolder);
                beam.name = "Beam Cap";
                beam.transform.localPosition = new Vector3(0f, floors[i].height + 3.8f, 0f);
            }

            // chamber 내부에 층별로 가는 버튼을 만든다
            {
                var go = Instantiate(buttonPrefab, chamberPanel);
                go.name = $"Button {floors[i].name}";
                go.transform.localPosition = new Vector2(0f, 280f - 110f * (floors.Length - 1 - i));
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
            //if (curIndex == value) return;
            curIndex = value;
            OnStatusUpdate?.Invoke(floors[curIndex].name);
        }
    }
    private int curIndex = -1;

    private void CalculateCurIndex(bool upward)
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
        Debug.Log($"Elevator MoveToFloor {floors[index].name}({index}) (wasMoving: {isMoving}) {chamberRbody.transform.localPosition.y:0.0} > {floors[index].height:0.0}");
        isMoving = true;
        chamberRbody.isKinematic = false;

        // 목표 높이
        float goalPos = floors[index].height;
        // 높이까지 남은 거리 offset
        float distance = goalPos - chamberRbody.transform.localPosition.y;
        bool upward = distance > 0f;

        // 엘리베이터 안에 있는 Rigidbody를 가져옴
        Collider[] carriedColliders = Physics.OverlapBox(chamberTrigger.bounds.center, chamberTrigger.bounds.extents, chamberTrigger.transform.rotation, LayerMask.GetMask("Player") | LayerMask.GetMask("Item"));
        List<Rigidbody> carriedRbodies = new();
        foreach (var c in carriedColliders)
            if (c.TryGetComponent<Rigidbody>(out var rigidbody) && rigidbody != chamberRbody)
                carriedRbodies.Add(rigidbody);

        while (Mathf.Abs(distance) > 0.001f)
        {
            // 남은 거리
            distance = goalPos - chamberRbody.transform.localPosition.y;
            // 이 물리프레임에 이동하는 거리
            float moveDistance = currentSpeed * Time.fixedDeltaTime;
            // 감속 시작 거리
            float decelerationDistance = (currentSpeed * currentSpeed) / (2f * chamberSpeed);

            if (Mathf.Abs(distance) <= decelerationDistance && currentSpeed != 0f) // 감속 거리 안이면 감속
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, chamberSpeed * Time.fixedDeltaTime);
            else if (Mathf.Abs(moveDistance) < Mathf.Abs(distance)) // 남은 거리가 있다면 가속 혹은 유지
                currentSpeed = Mathf.MoveTowards(currentSpeed, Mathf.Sign(distance) * chamberSpeed, chamberSpeed * Time.fixedDeltaTime);
            else currentSpeed = 0f;

            // 엘리베이터 및 내부 Rigidbody에 속도 적용
            chamberRbody.velocity = new Vector3(0f, currentSpeed, 0f);
            foreach (var rb in carriedRbodies)
                rb.velocity = new Vector3(rb.velocity.x, currentSpeed, rb.velocity.z);

            yield return new WaitForFixedUpdate();
            //Debug.Log($"MoveToFloor curSpeed: {currentSpeed:0.0}, moveD: {moveDistance:0.0} / dist: {distance:0.0} {chamberRbody.transform.localPosition.y:0.0} > {floors[index].height:0.0}");
            CalculateCurIndex(upward); // 실시간 층수 계산

            if (Mathf.FloorToInt(Time.time) % 2 == 0) OnStatusUpdate?.Invoke(upward ? "▲" : "▼"); // 방향 깜빡이
        }

        // 이동 끝: 엘리베이터를 목표 위치 및 속도로 리셋
        Debug.Log($"Elevator MoveToFloor Finished at {floors[index].name}({index}) {chamberRbody.transform.localPosition.y:0.0} == {floors[index].height:0.0}");
        isMoving = false;
        currentSpeed = 0f;
        chamberRbody.velocity = Vector3.zero;
        chamberRbody.transform.localPosition = new Vector3(0f, goalPos, 0f);
        chamberRbody.isKinematic = true; // 엘리베이터의 물리 끄기

        // 다른 변수들 리셋 및 도착 이벤트 (문 열기) 시행
        CurIndex = index;
        RequestOpenDoor();

        AudioSource.PlayClipAtPoint(floorArrivalSound, transform.position); // 도착 소리 재생
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

        if (doorOpenHang == 0f && doorOpen > 0f)
        {
            if (doorOpen == 1f)
            {
                AudioSource.PlayClipAtPoint(doorAutoCloseSound, transform.position);
            }

        }

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

        AudioSource.PlayClipAtPoint(doorOpenSound, transform.position);
        AudioSource.PlayClipAtPoint(doorOpenButtonSound, transform.position);
    }

    public void RequestCloseDoor()
    {
        doorOpenHang = 0f;

        AudioSource.PlayClipAtPoint(doorCloseSound, transform.position);
        AudioSource.PlayClipAtPoint(doorCloseButtonSound, transform.position);
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
