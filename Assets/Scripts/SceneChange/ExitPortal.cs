using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MySceneManager;

public class ExitPortal : MonoBehaviour
{
    [SerializeField]
    private SCENENAME targetScene = 0;

    protected virtual void PassThrough()
    {
        GameManager.Instance().Scene.ChangeScene(targetScene);
    }

    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("�浹");
        if (!_other.transform.parent.CompareTag("Player")) return;
        Debug.Log("�÷��̾� ����");
        PassThrough();
    }
}
