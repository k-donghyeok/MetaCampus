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
