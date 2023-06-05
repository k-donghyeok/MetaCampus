using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElevatorFloor : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtStatus = null;

    [SerializeField]
    private TMP_Text txtName = null;

    /// <summary>
    /// 엘리베이터 기기의 현재 위치 표시
    /// </summary>
    public void SetStatus(string text)
    {
        txtStatus.text = text;
    }

    /// <summary>
    /// 이 층의 이름 표시
    /// </summary>
    public void SetData(string name)
    {
        txtName.text = name;
    }

}
