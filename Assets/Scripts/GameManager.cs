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
        }
        else if (instance != this) Destroy(gameObject);
    }

    private void Start()
    {
        Initiate();
    }

    /// <summary>
    /// 점수 관리
    /// </summary>
    //public YeilManager Yeil { get; private set; } = null;

    /// <summary>
    /// 저장 관리
    /// </summary>
    public SaveManager Save { get; private set; } = null;

    public SpawnManager Spawn { get; private set; } = null;

    public MySceneManager Scene { get; private set; } = null;

    private void Initiate()
    {
        Debug.Log("게임매니저 생성후 매니저들 생성");
        Save = new SaveManager();
        //Yeil = new YeilManager();
        Spawn = new SpawnManager();
        Scene = new MySceneManager();

        Save.LoadFromPrefs();
        //Scene.MovePlayerToSpawn();
    }

}