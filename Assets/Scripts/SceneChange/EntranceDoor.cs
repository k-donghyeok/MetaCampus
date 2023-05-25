using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceDoor : Passage
{

   [SerializeField] private int myid =123; //내아이디 

   

    protected override void PassThrough()
    {
        base.PassThrough();
        //GameManager.Instance().Spawn.SaveSpawnPoint(myid);

        Debug.Log(GameManager.Instance().Scene);
        if(GameManager.Instance().Scene != null)
        {
            GameManager.Instance().Scene.ChangeScene(MySceneManager.SCENENAME.Interior);
        }
        


    }

    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("충돌");
        if (_other.tag == "Player")
        {
            Debug.Log("플레이어 맞음");
            PassThrough();
        }
    }

    
}
