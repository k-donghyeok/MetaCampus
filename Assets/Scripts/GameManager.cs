using System.Collections.Generic;
using UnityEngine;

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
    }

    
    public string UserID { get; set; } = "������";

    private Dictionary<string, bool> clearStatus
        = new Dictionary<string, bool>(); // �ǹ��� Ŭ���� ���� ����


}