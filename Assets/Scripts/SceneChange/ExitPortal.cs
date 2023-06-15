using UnityEngine;
using static MySceneManager;

public class ExitPortal : MonoBehaviour
{
    [SerializeField]
    private SCENENAME targetScene = 0;

    private void PassThrough()
    {
        if (!StageManager.Instance().IsExterior()) // ������ ��
        {
            bool tutorial = GetCurrentSceneName() == SCENENAME.Tutorial;
            if (!tutorial)
            {
                if (!GameManager.Instance().IsDaytime())
                    StageManager.Instance().ClearValidate(); // ���̸� Ŭ���� Ȯ�� �� ó��
                GameManager.Instance().ToggleDaytime(); // �㳷 ��ȯ
            }
            else
            {
                if (!GetCleared(SCENENAME.Tutorial)) // �� �� ����: ��Ʈ��
                {
                    SaveClear(SCENENAME.Tutorial); // ������ �� ������ ó��
                    GameManager.Instance().ToggleDaytime(); // �㳷 ��ȯ
                }
            }
        }
        GameManager.Instance().Scene.ChangeScene(targetScene);
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (!_other.transform.root.CompareTag("Player")) return;
        PassThrough();
    }
}
