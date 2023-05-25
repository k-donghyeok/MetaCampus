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

    //���൵���ִ��� Ȯ���ϴ� �Լ�

    //�������� ����Ȱ��ִ��� Ȯ���ϴ��Լ�


    /// <summary>
    /// ���� �ö���ִ� �÷��̾ ã�Ƽ� �÷��̾��� Ʈ�������� ��ȯ����
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
    /// ���� ����
    /// </summary>
    public YeilManager Yeil { get; private set; } = null;

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
        Yeil = new YeilManager();
        Spawn = new SpawnManager();
        Scene = new MySceneManager();

        Save.LoadFromPrefs();
        Scene.MovePlayerToSpawn();
    }

}