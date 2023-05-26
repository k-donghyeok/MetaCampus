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
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }


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
