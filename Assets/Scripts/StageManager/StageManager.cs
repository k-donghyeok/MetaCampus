using UnityEngine;

public class StageManager : MonoBehaviour
{
    private static StageManager instance = null;

    public static StageManager Instance() => instance;

    /// <summary>
    /// 타이머 관리
    /// </summary>
    public TimeManager Time { get; private set; } = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Initiate();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Update()
    {
        Time?.UpdateCountdown();
    }


    private void Initiate()
    {
        Time = new TimeManager();

        Time.StartCountdown();
    }
}
