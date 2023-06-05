using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [SerializeField]
    private FloorData[] floors = new FloorData[1];

    [SerializeField]
    private GameObject floorPrefab = null;

    private void Start()
    {
        // floors�� ������ŭ floorPrefab�� �ν��Ͻ��� �����
    }



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
}
