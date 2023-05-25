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

    //진행도가있는지 확인하는 함수

    //지도내용 변경된게있는지 확인하는함수


    /// <summary>
    /// 씬에 올라와있는 플레이어를 찾아서 플레이어의 트랜스폼을 반환해줌
    /// </summary>
    /// <returns></returns>
    public GameObject FindPlayerPosition()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    public GameObject FindAreaPosition(int exitID)
    {
        ExtiDoor[] exits =GetComponents<ExtiDoor>();
        foreach(var exit in exits)
        {
            if (exit.GetExitID() == exitID) return exit.gameObject;
        }
        Debug.LogError($"exitID {exitID} does not exist!");
        return null;

    }

    /// <summary>
    /// 점수 관리
    /// </summary>
    public YeilManager Yeil { get; private set; } = null;

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
        Yeil = new YeilManager();
        Spawn = new SpawnManager();
        Scene = new MySceneManager();

        Save.LoadFromPrefs();
        Scene.MovePlayerToSpawn();
    }

}