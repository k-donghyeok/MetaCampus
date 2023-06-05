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

    private void Initiate()
    {
        Debug.Log("���ӸŴ��� ������ �Ŵ����� ����");
        Save = new SaveManager();
        //Yeil = new YeilManager();
        Spawn = new SpawnManager();
        Scene = new MySceneManager();

        Save.LoadFromPrefs();
        //Scene.MovePlayerToSpawn();
    }

}