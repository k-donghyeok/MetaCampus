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
        /// 외부
        /// </summary>
        ///  
        Exterior = 0,
        /// <summary>
        /// 내부
        /// </summary>
        Interior = 1,
        /// <summary>
        /// 튜토리얼 건물
        /// </summary>
        Tutorial=2,

    }

    private int currentSceneID;

    /// <summary>
    /// 현재 씬 ID
    /// </summary>
    public int CurrentSceneID
    {
        get { return currentSceneID; }

        private set { currentSceneID = value; } 
    } 

    /// <summary>
    /// 씬 전환
    /// </summary>
    public void ChangeScene(SCENENAME _name)
    {
        Debug.Log("씬변경");

        StageManager.Instance().CheckClear();


        SceneManager.LoadScene((int)_name);


    }

    /// <summary>
    /// 플레이어를 저장된 스폰 포인트로 이동
    /// </summary>
    //public void MovePlayerToSpawn()
    //{
    //    Debug.Log($"MovePlayerToSpawn 실행됨 (외부: {StageManager.Instance().IsExterior()})");
    //    // 새로운 씬이 외부인 경우
    //    if (StageManager.Instance().IsExterior())
    //    {
    //        // 플레이어를 저장된 스폰 포인트로 이동
    //        GameManager.Instance().Spawn.SpawnPlayerToSavedLocation();
    //    }
    //}


}

    

