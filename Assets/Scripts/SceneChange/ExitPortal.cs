using UnityEngine;
using static MySceneManager;

public class ExitPortal : MonoBehaviour
{
    [SerializeField]
    private SCENENAME targetScene = 0;

    protected virtual void PassThrough()
    {
        if (!StageManager.Instance().IsExterior())
            StageManager.Instance().ClearValidate();
        GameManager.Instance().Scene.ChangeScene(targetScene);
    }

    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("충돌");
        if (!_other.transform.root.CompareTag("Player")) return;
        Debug.Log("플레이어 맞음");
        PassThrough();
    }
}
