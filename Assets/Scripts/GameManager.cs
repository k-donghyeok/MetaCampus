using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

/// <summary>
/// �̱��� ���ӸŴ���
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    /// <summary>
    /// ���ӸŴ��� �̱��� �ν��Ͻ�
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
    /// ���� ����
    /// </summary>
    //public YeilManager Yeil { get; private set; } = null;

    /// <summary>
    /// ���� ����
    /// </summary>
    public SaveManager Save { get; private set; } = null;

    public SpawnManager Spawn { get; private set; } = null;

    public MySceneManager Scene { get; private set; } = null;

    private bool daytime = true;

    /// <summary>
    /// ������ ����
    /// </summary>
    public bool IsDaytime() => daytime;

    /// <summary>
    /// ���� ��ȯ
    /// </summary>
    public void ToggleDaytime() => daytime = !daytime;

    private void Initiate()
    {
        daytime = true; // ������ �����ϸ� �� �ð�
        Debug.Log("���ӸŴ��� ������ �Ŵ����� ����");
        Save = new SaveManager();
        //Yeil = new YeilManager();
        Spawn = new SpawnManager();
        Scene = new MySceneManager();

        Save.LoadFromPrefs();
        //Scene.MovePlayerToSpawn();

        Initialized = true;
    }

    
    public string UserID { get; set; } = "������";

    private Dictionary<string, bool> clearStatus
        = new Dictionary<string, bool>(); // �ǹ��� Ŭ���� ���� ����

    public bool Paused { get; private set; } = false;

    public void GamePause()
    {
        if (Paused) return;
        Debug.Log("���� �Ͻ�����");
        Paused = true;
        Time.timeScale = 0f;
        var playerMove = PlayerManager.InstanceOrigin().GetComponent<DynamicMoveProvider>();
        if (playerMove) playerMove.moveSpeed = 0f;
    }

    public void GameUnpause()
    {
        if (!Paused) return;
        Debug.Log("���� �簳");
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