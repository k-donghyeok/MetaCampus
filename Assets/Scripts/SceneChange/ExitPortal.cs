using UnityEngine;
using static MySceneManager;

public class ExitPortal : MonoBehaviour
{
    [SerializeField]
    private SCENENAME targetScene = 0;

    private bool activated = false;

    private void PassThrough()
    {
        if (!StageManager.Instance().IsExterior()) // 내부일 때
        {
            bool tutorial = GetCurrentSceneName() == SCENENAME.Tutorial;
            if (!tutorial)
            {
                if (!GameManager.Instance().IsDaytime())
                    StageManager.Instance().ClearValidate(); // 밤이면 클리어 확인 및 처리
                GameManager.Instance().ToggleDaytime(); // 밤낮 전환
            }
            else
            {
                if (!GetCleared(SCENENAME.Tutorial)) // 깬 적 없음: 인트로
                {
                    SaveClear(SCENENAME.Tutorial); // 무조건 깬 것으로 처리
                    GameManager.Instance().ToggleDaytime(); // 밤낮 전환
                }
            }
        }
        GameManager.Instance().Scene.ChangeScene(targetScene);
        activated = true;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (activated) return;
        if (!_other.transform.root.CompareTag("Player")) return;
        PassThrough();
    }
}
