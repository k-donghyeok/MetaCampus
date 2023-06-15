using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

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

    public bool Initialized { get; private set; } = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) { Destroy(gameObject); return; }

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

    private bool daytime = true;

    /// <summary>
    /// 낮인지 여부
    /// </summary>
    public bool IsDaytime() => daytime;

    /// <summary>
    /// 낮밤 전환
    /// </summary>
    public void ToggleDaytime() => daytime = !daytime;

    private void Initiate()
    {
        daytime = true; // 게임이 시작하면 낮 시간
        Debug.Log("게임매니저 생성후 매니저들 생성");
        Save = new SaveManager();
        //Yeil = new YeilManager();
        Spawn = new SpawnManager();
        Scene = new MySceneManager();

        Save.LoadFromPrefs();
        //Scene.MovePlayerToSpawn();

        Initialized = true;
    }

    
    public string UserID { get; set; } = "강동혁";

    private Dictionary<string, bool> clearStatus
        = new Dictionary<string, bool>(); // 건물별 클리어 여부 저장

    public bool Paused { get; private set; } = false;

    public void GamePause()
    {
        if (Paused) return;
        Debug.Log("게임 일시정지");
        Paused = true;
        Time.timeScale = 0f;
        var playerMove = PlayerManager.InstanceOrigin().GetComponent<DynamicMoveProvider>();
        if (playerMove) playerMove.moveSpeed = 0f;
    }

    public void GameUnpause()
    {
        if (!Paused) return;
        Debug.Log("게임 재개");
        Paused = false;
        Time.timeScale = 1f;
        var playerMove = PlayerManager.InstanceOrigin().GetComponent<DynamicMoveProvider>();
        if (playerMove) playerMove.moveSpeed = StageManager.Instance().IsExterior() ? 5f : 3f;
    }

    public void StartIntro()
    {
        Scene.ChangeScene(MySceneManager.SCENENAME.Tutorial);
    }
}