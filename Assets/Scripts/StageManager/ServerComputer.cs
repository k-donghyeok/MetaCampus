using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerComputer : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("�浹");
        if(_other.transform.parent.CompareTag("Player"))
        {
            Debug.Log("�÷��̾����");
            StageManager.Instance().IsClear=true;
            Debug.Log("�������� �������� : "+StageManager.Instance().IsClear);
            StageManager.Instance().IsPlayerInServerRoom=true;

            //������ ����� �������尪 ����

            // �̸��Է� ui Ȱ��ȭ


        }
    }

    private void OnTriggerExit(Collider _other)
    {
        Debug.Log("�浹����");
        if (_other.transform.parent.CompareTag("Player"))
        {
            StageManager.Instance().IsPlayerInServerRoom = false;
        }
    }
}
