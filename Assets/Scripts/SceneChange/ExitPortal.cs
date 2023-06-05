using UnityEngine;
using static MySceneManager;

public class ExitPortal : MonoBehaviour
{
    [SerializeField]
    private SCENENAME targetScene = 0;

    protected virtual void PassThrough()
    {
        if (!StageManager.Instance().IsExterior()) // 내부일 때
        {
            if (!GameManager.Instance().IsDaytime())
                StageManager.Instance().ClearValidate(); // 밤이면 클리어 확인 및 처리

            GameManager.Instance().ToggleDaytime(); // 밤낮 전환
        }
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
