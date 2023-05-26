using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private bool exterior = true;

    public bool IsExterior() => exterior;

    public bool IsClear { get; set; } = false;

    public bool IsPlayerInServerRoom { get; set; } =false;

    public string UserID { get; set; } = "������";

    private static StageManager instance = null;

    public static StageManager Instance() => instance;

    /// <summary>
    /// �� ���������� �� �̸��� ��ȯ
    /// </summary>
    public string GetName() => gameObject.scene.name;

    
    /// <summary>
    /// Ÿ�̸� ����
    /// </summary>
    public TimeManager Time { get; private set; } = null;

    public LockManager Lock { get; private set; } = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (!IsExterior()) InitiateInterior();
        else InitiateExterior();
        OnStageLoad?.Invoke(this);
    }

    private void OnDestroy()
    {
        OnStageUnload?.Invoke(this);
        if (instance == this)
            instance = null;
    }

    public delegate void StageEvent(StageManager stage);

    /// <summary>
    /// ���ο� ���������� <see cref="Start"/>�� �θ��� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public StageEvent OnStageLoad = null;
    /// <summary>
    /// ���� �ִ� ���������� �������� <see cref="OnDestroy"/>�� �θ� �� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public StageEvent OnStageUnload = null;


    private void Update()
    {
        if (IsExterior()) return;

        if (IsPlayerInServerRoom) return;

        Time?.UpdateCountdown();
    }

    private void InitiateInterior()
    {
        Time = new TimeManager();
        Lock = new LockManager();

        Time.StartCountdown();
        
    }


    public void CheckClear()
    {
        if(!IsExterior())
        {
            Debug.Log("�������� ����" + IsClear);
            Debug.Log("���ѽð��ȿ� Ż�⼺������" + Time.IsCountdownComplete);
            if (IsClear && Time.IsCountdownComplete)
            {
                //����� ���� ����
                UpdateWorldMap();
                //���ü����� �ð� �ø��� 
                int score = Mathf.RoundToInt(Time.RemainingTime * 100f); // 100���� 1�� ����
                Debug.Log("�����ð� : " + score);
                StartCoroutine(SetScoreCoroutine(UserID, score));
            }
        }
       
        
    }

    private void UpdateWorldMap()
    {

    }
    
    private IEnumerator SetScoreCoroutine(string _id, int _score)
    {
        Debug.Log(_id);
        Debug.Log(_score);
        WWWForm form = new WWWForm();
        form.AddField("id", _id);
        form.AddField("score", _score);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/addscore.php", form))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("���ͳݿ����� Ȯ���ϼ���");
                yield break;
            }

            string data = www.downloadHandler.text;
            // int.TryParse(data, out int result);
            //Debug.Log(www.downloadHandler.isDone);
            //Debug.Log(www.downloadHandler.error);
            //Debug.Log(www.downloadHandler.text);
            Debug.Log("����ȯ �������� : " + int.TryParse(data, out int result));
            int intdata = result;


            if (intdata == 0) //���̵����� ������������ �������� ���ż���
            {
                Debug.Log($"id : {_id} score : {_score} ���ż���");
            }
            if (intdata == 1) //���̵�� ������ ������ ������
            {
                Debug.Log($"id : {_id} score : {_score} �ش� ���̵��� ������ ����� ������ ������");
            }
            if (intdata == 2) //�����̵� �������� ����
            {
                Debug.Log($"id : {_id} score : {_score} �����̵� ���ż���");
            }


        }
    }
    private void InitiateExterior()
    {
        GameManager.Instance().Spawn.SpawnPlayerToSavedLocation();
    }

}
