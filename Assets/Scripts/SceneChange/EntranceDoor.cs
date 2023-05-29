using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceDoor : SpawnPoint
{
    protected override void PassThrough()
    {
        base.PassThrough();
        GameManager.Instance().Scene.ChangeScene(MySceneManager.SCENENAME.Interior);

    }

    
}
