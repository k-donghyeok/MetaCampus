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
        /// <summary>
        /// ����
        /// </summary>
        Interior = 0,
        /// <summary>
        /// �ܺ�
        /// </summary>
        Exterior = 1
       
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

    

