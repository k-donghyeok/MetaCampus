using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ServerComputer : MonoBehaviour
{
    [SerializeField] GameObject canvas = null;
    [SerializeField] GameObject detailUiPrefab = null;
    [SerializeField] Transform parentUi = null;

    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("충돌");
        if (_other.transform.parent.CompareTag("Player"))
        {
            Debug.Log("플레이어맞음");
            StageManager.Instance().IsClear = true;
            Debug.Log("성적수정 성공여부: " + StageManager.Instance().IsClear);
            StageManager.Instance().IsPlayerInServerRoom = true;

            StartCoroutine(GetScoreLeaderboardCoroutine());
        }
    }

    private IEnumerator GetScoreLeaderboardCoroutine()
    {
        yield return StageManager.Instance().GetScoreCoroutine();
        Debug.Log("스테이지매니저의 코루틴사용해서 성적 수정후");
        // 서버에서 받아온 데이터 사용
        var dataScores = StageManager.Instance().dataScores;
        Debug.Log(dataScores.Count);
        // 서버에 저장된 리더보드값 띄우기
        canvas.SetActive(true);
        InstanceUi(dataScores);
    }

    private void OnTriggerExit(Collider _other)
    {
        Debug.Log("충돌해제");
        if (_other.transform.parent.CompareTag("Player"))
        {
            StageManager.Instance().IsPlayerInServerRoom = false;

            canvas.SetActive(false);
        }
    }

    private void InstanceUi(List<StageManager.DataScore> _dataScores)
    {
        Vector3 pos = new Vector3(0f,190f,0f);
        for(int i=0;i<_dataScores.Count;++i)
        {
            GameObject go =Instantiate(detailUiPrefab, parentUi);
            go.transform.localPosition = pos;
            pos = pos - new Vector3(0f, 80f, 0f);
        }
    }

     
}
