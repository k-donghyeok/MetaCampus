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

    private void Start()
    {
        CheckSPAWN("asd");
        //체크진행도
        //체크지도내용
    }

    //스폰포인트가있는지확인하는함수
    private void CheckSPAWN(string _spawnid)
    {
        int a = Save.LoadValue(_spawnid, -1);

        if( a ==-1) //세이브가 없을떄
        {
            return;
        }

        //세이브가 있을떄
        if(a !=-1)
        {
            //플레이어 포지션을 id의 포지션으로 변경
        }

    }
        

    //진행도가있는지 확인하는 함수

        //지도내용 변경된게있는지 확인하는함수

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
        Save = new SaveManager();
        Yeil = new YeilManager();
        Spawn = new SpawnManager();
        Scene = new MySceneManager();
       
        Save.Initialize();
    }

}
