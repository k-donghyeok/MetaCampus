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
            SCENENAME.Tutorial => "인문대학",
            SCENENAME.Engineering => "공가대학",
            SCENENAME.Medical => "의과대학",
            SCENENAME.Arts => "예술대학",
            SCENENAME.Boss => "대학본부",
            _ => "캠퍼스",
        };
    }

    /// <summary>
    /// 씬 전환
    /// </summary>
    public void ChangeScene(SCENENAME _name)
    {
        Debug.Log("씬변경");

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

    

