using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager
{
    public enum SCENENAME : int
    {
        /// <summary>
        /// 내부
        /// </summary>
        Interior = 0,
        /// <summary>
        /// 외부
        /// </summary>
        Exterior = 1
       
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
        
        SceneManager.LoadScene((int)_name);

        // exterior 값은 유니티 에디터 Inspector에서 설정하는 거라 코드에서 고칠 필요도 없고, 고쳐서도 안 됨.
        // 애초에 외부에서 수정하지 말라고 캡슐화한 겁니다.
        /*
        if(_name==SCENENAME.Exterior)
        {
            StageManager.Instance().ChangeExterior(true);
        }

        if(_name!=SCENENAME.Exterior)
        {
            StageManager.Instance().ChangeExterior(false);
        }
        */

        MovePlayerToSpawn();
    }

    /// <summary>
    /// 플레이어를 저장된 스폰 포인트로 이동
    /// </summary>
    public void MovePlayerToSpawn()
    {
            Debug.Log($"MovePlayerToSpawn 실행됨 (외부: {StageManager.Instance().IsExterior()})") ;
        // 새로운 씬이 외부인 경우
        if (StageManager.Instance().IsExterior())
        {
            // 플레이어를 저장된 스폰 포인트로 이동
            GameManager.Instance().Spawn.SpawnPlayerToSavedLocation();
        }
    }


}

    

