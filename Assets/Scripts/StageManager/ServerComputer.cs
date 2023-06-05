using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class ServerComputer : MonoBehaviour
{
    [SerializeField] GameObject canvas = null;
    [SerializeField] GameObject detailUiPrefab = null;
    [SerializeField] Transform parentUi = null;

    private List<DataScore> dataScores = new();

    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("충돌");
        if (_other.transform.root.CompareTag("Player"))
        {
            Debug.Log("플레이어맞음");
            StageManager.Instance().IsClear = true;
            Debug.Log("성적수정 성공여부: " + StageManager.Instance().IsClear);
            StageManager.Instance().IsPlayerInServerRoom = true;

            StartCoroutine(OpenLeaderboardCoroutine());
        }
    }

    private IEnumerator OpenLeaderboardCoroutine()
    {
        yield return DownloadScoreCoroutine();
        Debug.Log("스테이지매니저의 코루틴사용해서 성적 수정후");
        // 서버에서 받아온 데이터 사용
        Debug.Log(dataScores.Count);
        // 서버에 저장된 리더보드값 띄우기
        canvas.SetActive(true);
        yield return InitiateUI(dataScores);
    }

    private IEnumerator InitiateUI(List<DataScore> _dataScores)
    {
        Vector3 pos = new Vector3(0f,190f,0f);
        int j = 0;
        for (int i = 0; i < _dataScores.Count; ++i)
        {
            GameObject go = Instantiate(detailUiPrefab, parentUi);
            go.transform.localPosition = pos;
            pos = pos - new Vector3(0f, 80f, 0f);
            ++j;
            if (j > 9) { yield return null; j = 0; }
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        Debug.Log("충돌해제");
        if (_other.transform.root.CompareTag("Player"))
        {
            StageManager.Instance().IsPlayerInServerRoom = false;

            canvas.SetActive(false);
        }
    }

    private IEnumerator DownloadScoreCoroutine()
    {
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/getscore.php", ""))
        {
            yield return www.SendWebRequest();
            Debug.Log("서버와 통신 후");
            if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            string data = www.downloadHandler.text;
            dataScores = JsonConvert.DeserializeObject<List<DataScore>>(data);
        }
    }

    [Serializable]
    private class DataScore
    {
        public string id;
        public int score;
    }
}
