using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerComputer : MonoBehaviour
{
    [SerializeField]
    private Image monitorCanvas;

    [SerializeField]
    private TMP_Text monitorText;

    private void Start()
    {
        if (GameManager.Instance().IsDaytime())
        {
            monitorCanvas.color = Color.black;
            monitorText.text = "See you later! :)";
        }
    }

    public void OnUsed()
    {
        if (GameManager.Instance().IsDaytime()) return;
        if (StageManager.Instance().IsClear) return;
        if (StageManager.Instance().Time.IsCountdownComplete)
        {
            monitorText.text = "Too late! x(";
            return;
        }
        StageManager.Instance().IsClear = true;
        monitorCanvas.color = new Color(0.2f, 0.2f, 1f);
        monitorText.text = "Input Received!";
    }


    /*
    [SerializeField] GameObject canvas = null;
    [SerializeField] GameObject detailUiPrefab = null;
    [SerializeField] Transform parentUi = null;


    private List<DataScore> dataScores = new();

    

    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("�浹");
        if (_other.transform.root.CompareTag("Player"))
        {
            Debug.Log("�÷��̾����");
            StageManager.Instance().IsClear = true;
            Debug.Log("�������� ��������: " + StageManager.Instance().IsClear);
            //StageManager.Instance().IsPlayerInServerRoom = true;

            // StartCoroutine(OpenLeaderboardCoroutine());
        }
    }


    private void OnTriggerExit(Collider _other)
    {
        //Debug.Log("�浹����");
        if (_other.transform.root.CompareTag("Player"))
        {
            //StageManager.Instance().IsPlayerInServerRoom = false;

            //canvas.SetActive(false);
        }
    }

    private IEnumerator OpenLeaderboardCoroutine()
    {
        yield return DownloadScoreCoroutine();
        Debug.Log("���������Ŵ����� �ڷ�ƾ����ؼ� ���� ������");
        // �������� �޾ƿ� ������ ���
        Debug.Log(dataScores.Count);
        // ������ ����� �������尪 ����
        canvas.SetActive(true);
        yield return InitiateUI(dataScores);
    }

    private IEnumerator InitiateUI(List<DataScore> _dataScores)
    {
        Vector3 pos = new Vector3(0f, 190f, 0f);
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
    private IEnumerator DownloadScoreCoroutine()
    {
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/getscore.php", ""))
        {
            yield return www.SendWebRequest();
            Debug.Log("������ ��� ��");
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

    */
}
