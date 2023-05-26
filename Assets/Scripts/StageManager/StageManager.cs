using UnityEngine;
using UnityEngine.XR.Management;
using UnityEngine.XR;
using Unity.VisualScripting;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private bool exterior = true;

    public bool IsExterior() => exterior;

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
        Time?.UpdateCountdown();
    }

    private void InitiateInterior()
    {
        Time = new TimeManager();
        Lock = new LockManager();

        Time.StartCountdown();
        
    }
    private void InitiateExterior()
    {
        GameManager.Instance().Spawn.SpawnPlayerToSavedLocation();
    }

}
