using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtiDoor : Passage
{
    [SerializeField] private int myID = 456;

    [SerializeField] private int ExitID = 123;

    public int GetExitID() => ExitID;
    //
    protected override void PassThrough()
    {
        base.PassThrough();
        GameManager.Instance().Spawn.SaveSpawnPoint(GetExitID());
        GameManager.Instance().Scene.ChangeScene(MySceneManager.SCENENAME.Exterior);
    }



    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("충돌");
        if(_other.tag == "Player")
        {
            Debug.Log("플레이어 맞음");
            PassThrough();
        }
       
    }
}
