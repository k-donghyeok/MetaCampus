using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceDoor : Passage
{

    private int myid =123; //내아이디 

    private int exitID = -123; //이문의 아이디가 아니고 내부에 생성될곳의 아이디

    protected override void PassThrough()
    {
        base.PassThrough();
        GameManager.Instance().Spawn.TestSave(exitID);


    }

    private void OnTriggerEnter(Collider _other)
    {
        if(_other.tag == "Player")
        {
            PassThrough();
            GameManager.Instance().Scene.ChangeScene(MySceneManager.SCENENAME.Interior);
        }
    }

    
}
