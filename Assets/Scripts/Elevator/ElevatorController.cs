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
        // floors의 개수만큼 floorPrefab의 인스턴스를 만든다
    }



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
}
