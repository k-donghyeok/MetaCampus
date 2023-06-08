using UnityEngine;
using static MySceneManager;

public class ExitPortal : MonoBehaviour
{
    [SerializeField]
    private SCENENAME targetScene = 0;

    protected virtual void PassThrough()
    {
        if (!StageManager.Instance().IsExterior()) // ������ ��
        {
            if (!GameManager.Instance().IsDaytime())
                StageManager.Instance().ClearValidate(); // ���̸� Ŭ���� Ȯ�� �� ó��

            GameManager.Instance().ToggleDaytime(); // �㳷 ��ȯ
        }
        GameManager.Instance().Scene.ChangeScene(targetScene);
    }

    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("�浹");
        if (!_other.transform.root.CompareTag("Player")) return;
        Debug.Log("�÷��̾� ����");
        PassThrough();
    }
}
