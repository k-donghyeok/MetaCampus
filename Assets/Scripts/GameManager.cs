using UnityEngine;

/// <summary>
/// 싱글톤 게임매니저
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    /// <summary>
    /// 게임매니저 싱글톤 인스턴스
    /// </summary>
    public static GameManager Instance() => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Initiate();
        }
        else if (instance != this) Destroy(gameObject);
    }

    /// <summary>
    /// 점수 관리
    /// </summary>
    public YeilManager Yeil { get; private set; } = null;

    /// <summary>
    /// 저장 관리
    /// </summary>
    public SaveManager Save { get; private set; } = null;

    public LockManager Lock { get; private set; } = null;
    private void Initiate()
    {
        Save = new SaveManager();
        Yeil = new YeilManager();
        Lock = new LockManager(); 
       
        Save.Initialize();
    }

}
