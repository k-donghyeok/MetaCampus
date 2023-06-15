using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class StageManager : MonoBehaviour
{

    [SerializeField]
    private bool exterior = true;

    [SerializeField]
    private float countdownDuration = 180f;

    public bool IsExterior() => exterior;

    public bool IsClear { get; set; } = false;

    public bool IsPlayerInServerRoom { get; set; } = false;

    public bool Initialized { get; private set; } = false;


    private static StageManager instance = null;

    public static StageManager Instance() => instance;

    /// <summary>
    /// ��꿡 ���Ǵ� �� ���������� �� �̸��� ��ȯ
    /// </summary>
    public string GetID() => MySceneManager.GetCurrentSceneName().ToString();

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
        if (!IsExterior())
        {
            InitiateInterior();
        }
        else
        {
            InitiateExterior();
        }

        GetComponentInChildren<LightManager>().Initialize(GameManager.Instance().IsDaytime());

        OnStageLoad?.Invoke(this);
        Initialized = true;
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
        if (!Initialized) return;
        if (IsExterior()) return;

        if (!GameManager.Instance().IsDaytime() && !IsPlayerInServerRoom)
            Time.UpdateCountdown();
    }

    private void InitiateInterior()
    {
        Time = new TimeManager(countdownDuration);
        Lock = new LockManager();

        Time.StartCountdown();
    }

    /// <summary>
    /// Ŭ���� �ߴ��� Ȯ�� �� ����
    /// </summary>
    public void ClearValidate()
    {
        Debug.Log("�������� ����" + IsClear);
        Debug.Log("���ѽð��ȿ� Ż�⼺������" + !Time.IsCountdownComplete);
        if (!IsClear || Time.IsCountdownComplete) return; // Ŭ���� ����

        //����� ���� ����
        //UpdateWorldMap();
        //���ü����� �ð� �ø��� 
        //int score = Mathf.RoundToInt(Time.RemainingTime * 100f); // 100���� 1�� ����
        //Debug.Log("�����ð� : " + score);
        //StartCoroutine(UploadScoreCoroutine(GameManager.Instance().UserID, score));

        // ���൵ ����
        SaveClear();
    }

    public void SaveClear()
    {
        string buildingName = GetID();
        GameManager.Instance().Save.SaveValue(buildingName, true);
    }

    private IEnumerator UploadScoreCoroutine(string _id, int _score)
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


    private void UpdateWorldMap()
    {

    }


    private void InitiateExterior()
    {
        GameManager.Instance().Spawn.SpawnPlayerToSavedLocation();
    }
}