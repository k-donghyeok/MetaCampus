using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceDoor : Passage
{

    private int myid =123; //�����̵� 

    private int exitID = -123; //�̹��� ���̵� �ƴϰ� ���ο� �����ɰ��� ���̵�

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
