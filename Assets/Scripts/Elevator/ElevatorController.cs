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

    [Header("Prefabs")]
    [SerializeField]
    private GameObject floorPrefab = null;
    [SerializeField]
    private GameObject buttonPrefab = null;

    public delegate void StatusUpdateHandler(string status);

    public StatusUpdateHandler OnStatusUpdate = null;

    private void Start()
    {
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
            }

            // chamber 내부에 층별로 가는 버튼을 만든다
            {
                var go = Instantiate(buttonPrefab, chamberPanel);
                go.name = $"Button {floors[i].name}";
                go.transform.localPosition = new Vector2(0f, 400f - 110f * (floors.Length - 1 - i));
                var button = go.GetComponent<ElevatorButtonFloor>();
                button.Initiate(this, i, floors[i].name);
            }
        }

        // 엘리베이터 상태가 변경될때 칸 안의 상태 텍스트를 바꾼다
        OnStatusUpdate += (status) => chamberTxtStatus.text = status;

        // 첫 번째 층으로 엘리베이터를 위치

        // 상태 메시지 초기화
        OnStatusUpdate?.Invoke(floors[0].name);
    }


    public void RequestMoveToFloor(int index, bool exterior)
    {
        //floors[index].height
        if (exterior) RequestOpenDoor();
    }

    public void RequestOpenDoor()
    {

    }

    public void RequestCloseDoor()
    {

    }


}
