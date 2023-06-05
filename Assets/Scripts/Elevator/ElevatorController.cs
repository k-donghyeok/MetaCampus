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

    [Header("Prefabs")]
    [SerializeField]
    private GameObject floorPrefab = null;
    [SerializeField]
    private GameObject buttonPrefab = null;

    public delegate void StatusUpdateHandler(string status);

    public StatusUpdateHandler OnStatusUpdate = null;

    private void Start()
    {
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
            }

            // chamber ���ο� ������ ���� ��ư�� �����
            {
                var go = Instantiate(buttonPrefab, chamberPanel);
                go.name = $"Button {floors[i].name}";
                go.transform.localPosition = new Vector2(0f, 400f - 110f * (floors.Length - 1 - i));
                var button = go.GetComponent<ElevatorButtonFloor>();
                button.Initiate(this, i, floors[i].name);
            }
        }

        // ���������� ���°� ����ɶ� ĭ ���� ���� �ؽ�Ʈ�� �ٲ۴�
        OnStatusUpdate += (status) => chamberTxtStatus.text = status;

        // ù ��° ������ ���������͸� ��ġ

        // ���� �޽��� �ʱ�ȭ
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
