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

    public string UserID { get; set; } = "강동혁";

    private static StageManager instance = null;

    public static StageManager Instance() => instance;

    /// <summary>
    /// 이 스테이지의 씬 이름을 반환
    /// </summary>
    public string GetName() => gameObject.scene.name;

    
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
    /// 새로운 스테이지가 <see cref="Start"/>를 부르고 발생하는 이벤트
    /// </summary>
    public StageEvent OnStageLoad = null;
    /// <summary>
    /// 원래 있던 스테이지가 없어지며 <see cref="OnDestroy"/>를 부를 때 발생하는 이벤트
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
            Debug.Log("성적수정 여부" + IsClear);
            Debug.Log("제한시간안에 탈출성공여부" + Time.IsCountdownComplete);
            if (IsClear && Time.IsCountdownComplete)
            {
                //월드맵 지도 갱신
                UpdateWorldMap();
                //로컬서버에 시간 올리기 
                int score = Mathf.RoundToInt(Time.RemainingTime * 100f); // 100분의 1초 단위
                Debug.Log("남은시간 : " + score);
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
    private void InitiateExterior()
    {
        GameManager.Instance().Spawn.SpawnPlayerToSavedLocation();
    }

}
