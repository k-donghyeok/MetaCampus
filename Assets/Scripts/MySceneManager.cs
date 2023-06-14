using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.Oculus;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Management;

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
        Debug.Log("������");

        SceneManager.LoadScene((int)_name);
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

    

