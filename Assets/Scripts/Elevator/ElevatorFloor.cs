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

    private int index = -1;

    private ElevatorController owner = null;

    public void SetOwner(ElevatorController owner)
        => this.owner = owner;

    /// <summary>
    /// �⺻ ������ �Է�
    /// </summary>
    public void Initiate(ElevatorController owner,
        int index, string name)
    {
        this.owner = owner;
        this.index = index;
        txtName.text = name;
    }

    /// <summary>
    /// ���������� ����� ���� ��ġ ǥ��
    /// </summary>
    public void SetStatus(string text)
    {
        txtStatus.text = text;
    }


    public void CallElevator()
    {
        owner.MoveToFloor(index);
    }

}
