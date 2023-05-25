using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtiDoor : Passage
{

    //
    protected override void PassThrough()
    {
        base.PassThrough();
        GameManager.Instance().Spawn.SaveSpawnPoint(1,Vector3.zero);
    }



    private void OnTriggerEnter(Collider _other)
    {
        if (_other.tag == "Player")
        {
            GameManager.Instance().Scene.ChangeScene(MySceneManager.SCENENAME.Exterior);
        }
    }
}
