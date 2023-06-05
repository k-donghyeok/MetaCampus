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
    /// ���������� ����� ���� ��ġ ǥ��
    /// </summary>
    public void SetStatus(string text)
    {
        txtStatus.text = text;
    }

    /// <summary>
    /// �� ���� �̸� ǥ��
    /// </summary>
    public void SetData(string name)
    {
        txtName.text = name;
    }

}
