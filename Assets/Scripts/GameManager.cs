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
            Initiate();
        }
        else if (instance != this) Destroy(gameObject);
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public YeilManager Yeil { get; private set; } = null;

    /// <summary>
    /// ���� ����
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
