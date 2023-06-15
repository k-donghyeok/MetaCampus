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
    /// 계산에 사용되는 이 스테이지의 씬 이름을 반환
    /// </summary>
    public string GetID() => MySceneManager.GetCurrentSceneName().ToString();

    /// <summary>
    /// 타이머 관리
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
    /// 새로운 스테이지가 <see cref="Start"/>를 부르고 발생하는 이벤트
    /// </summary>
    public StageEvent OnStageLoad = null;
    /// <summary>
    /// 원래 있던 스테이지가 없어지며 <see cref="OnDestroy"/>를 부를 때 발생하는 이벤트
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
    /// 클리어 했는지 확인 후 저장
    /// </summary>
    public void ClearValidate()
    {
        Debug.Log("성적수정 여부" + IsClear);
        Debug.Log("제한시간안에 탈출성공여부" + !Time.IsCountdownComplete);
        if (!IsClear || Time.IsCountdownComplete) return; // 클리어 실패

        //월드맵 지도 갱신
        //UpdateWorldMap();
        //로컬서버에 시간 올리기 
        //int score = Mathf.RoundToInt(Time.RemainingTime * 100f); // 100분의 1초 단위
        //Debug.Log("남은시간 : " + score);
        //StartCoroutine(UploadScoreCoroutine(GameManager.Instance().UserID, score));

        // 진행도 저장
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
                Debug.Log("인터넷연결을 확인하세요");
                yield break;
            }

            string data = www.downloadHandler.text;
            // int.TryParse(data, out int result);
            //Debug.Log(www.downloadHandler.isDone);
            //Debug.Log(www.downloadHandler.error);
            //Debug.Log(www.downloadHandler.text);
            Debug.Log("형변환 성공여부 : " + int.TryParse(data, out int result));
            int intdata = result;


            if (intdata == 0) //아이디있음 서버점수보다 높은점수 갱신성공
            {
                Debug.Log($"id : {_id} score : {_score} 갱신성공");
            }
            if (intdata == 1) //아이디는 있지만 점수가 별로임
            {
                Debug.Log($"id : {_id} score : {_score} 해당 아이디의 서버에 저장된 점수가 더높음");
            }
            if (intdata == 2) //새아이디에 점수갱신 성공
            {
                Debug.Log($"id : {_id} score : {_score} 새아이디 갱신성공");
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