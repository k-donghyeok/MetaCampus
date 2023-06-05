using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    private GameObject floorPrefab = null;

    private void Start()
    {
        // floors�� ������ŭ floorPrefab�� �ν��Ͻ��� �����
        for (int i = 0; i < floors.Length; ++i)
        {
            var go = Instantiate(floorPrefab, transform);
            go.name = $"Floor {floors[i].name}";
            go.transform.localPosition = new Vector3(0f, floors[i].height, 0f);
            var floor = go.GetComponent<ElevatorFloor>();
            floor.Initiate(this, i, floors[i].name);
        }
    }


    public void MoveToFloor(int index)
    {
        //floors[index].height
    }


}
