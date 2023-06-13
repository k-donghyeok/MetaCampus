using UnityEngine;
using static MySceneManager;

public class ExitPortal : MonoBehaviour
{
    [SerializeField]
    private SCENENAME targetScene = 0;

    private void PassThrough()
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
        if (!_other.transform.root.CompareTag("Player")) return;
        PassThrough();
    }
}
