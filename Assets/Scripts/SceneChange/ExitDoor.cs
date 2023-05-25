using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : SpawnPoint
{
    protected override void PassThrough()
    {
        GameManager.Instance().Scene.ChangeScene(MySceneManager.SCENENAME.Exterior);
    }
}
