using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerComputer : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("충돌");
        if(_other.transform.parent.CompareTag("Player"))
        {
            Debug.Log("플레이어맞음");
            StageManager.Instance().IsClear=true;
            Debug.Log("성적수정 성공여부 : "+StageManager.Instance().IsClear);
            StageManager.Instance().IsPlayerInServerRoom=true;

            //서버에 저장된 리더보드값 띄우기

            // 이름입력 ui 활성화


        }
    }

    private void OnTriggerExit(Collider _other)
    {
        Debug.Log("충돌해제");
        if (_other.transform.parent.CompareTag("Player"))
        {
            StageManager.Instance().IsPlayerInServerRoom = false;
        }
    }
}
