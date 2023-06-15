using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager
{
    public enum SCENENAME : int
    {
        Exterior = 0,
        Tutorial = 1,
        Engineering = 2,
        Medical = 3,
        Arts = 4,
        Boss = 5
    }

    public static SCENENAME GetCurrentSceneName()
        => (SCENENAME)SceneManager.GetActiveScene().buildIndex;

    public static string GetDisplaySceneName(SCENENAME scene)
    {
        return scene switch
        {
            SCENENAME.Tutorial => "�ι�����",
            SCENENAME.Engineering => "��������",
            SCENENAME.Medical => "�ǰ�����",
            SCENENAME.Arts => "��������",
            SCENENAME.Boss => "���к���",
            _ => "ķ�۽�",
        };
    }

    /// <summary>
    /// �� ��ȯ
    /// </summary>
    public void ChangeScene(SCENENAME _name)
    {
        Debug.Log($"��� ��ȯ: {GetCurrentSceneName()} => {_name}");

        SceneManager.LoadScene((int)_name);
    }

    public static void SaveClear(SCENENAME name)
    {
        GameManager.Instance().Save.SaveValue(GetClearID(name), true);
    }

    private static string GetClearID(SCENENAME name)
        => $"Cleared{name}";

    public static bool GetCleared(SCENENAME name)
    {
        return GameManager.Instance().Save.LoadValue(GetClearID(name), false);
    }

    /// <summary>
    /// �÷��̾ ����� ���� ����Ʈ�� �̵�
    /// </summary>
    //public void MovePlayerToSpawn()
    //{
    //    Debug.Log($"MovePlayerToSpawn ����� (�ܺ�: {StageManager.Instance().IsExterior()})");
    //    // ���ο� ���� �ܺ��� ���
    //    if (StageManager.Instance().IsExterior())
    //    {
    //        // �÷��̾ ����� ���� ����Ʈ�� �̵�
    //        GameManager.Instance().Spawn.SpawnPlayerToSavedLocation();
    //    }
    //}


}



