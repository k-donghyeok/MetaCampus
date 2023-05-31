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
        Debug.Log("�浹");
        if (_other.transform.parent.CompareTag("Player"))
        {
            Debug.Log("�÷��̾����");
            StageManager.Instance().IsClear = true;
            Debug.Log("�������� ��������: " + StageManager.Instance().IsClear);
            StageManager.Instance().IsPlayerInServerRoom = true;

            StartCoroutine(GetScoreLeaderboardCoroutine());
        }
    }

    private IEnumerator GetScoreLeaderboardCoroutine()
    {
        yield return StageManager.Instance().GetScoreCoroutine();
        Debug.Log("���������Ŵ����� �ڷ�ƾ����ؼ� ���� ������");
        // �������� �޾ƿ� ������ ���
        var dataScores = StageManager.Instance().dataScores;
        Debug.Log(dataScores.Count);
        // ������ ����� �������尪 ����
        canvas.SetActive(true);
        InstanceUi(dataScores);
    }

    private void OnTriggerExit(Collider _other)
    {
        Debug.Log("�浹����");
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
