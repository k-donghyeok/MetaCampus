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

    private int currentSceneID;

    /// <summary>
    /// ���� �� ID
    /// </summary>
    public int CurrentSceneID
    {
        get { return currentSceneID; }

        private set { currentSceneID = value; } 
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

    

