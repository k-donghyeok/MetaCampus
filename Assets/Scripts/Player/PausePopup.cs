using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static PauseMenuManager;

public class PausePopup : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtAction;
    [SerializeField]
    private TMP_Text txtMessage;

    private PopupAction curAction;

    public void SetAction(PopupAction action)
    {
        curAction = action;
        switch (curAction)
        {
            case PopupAction.Retry:
                txtAction.text = "��õ��Ͻðڽ��ϱ�?";
                txtMessage.text = "�� �ǹ� ��������\n��������� ������� �ʽ��ϴ�.";
                break;
            case PopupAction.Escape:
                txtAction.text = "�����ðڽ��ϱ�?";
                txtMessage.text = "�� �ǹ� ��������\n��������� ������� �ʽ��ϴ�.";
                break;
            case PopupAction.Reset:
                txtAction.text = "���̺� �ʱ�ȭ�ϰڽ��ϱ�?";
                txtMessage.text = "��� ��������� �����˴ϴ�.";
                break;
            case PopupAction.Exit:
                txtAction.text = "������ �����Ͻðڽ��ϱ�?";
                txtMessage.text = StageManager.Instance().IsExterior() ? "" : "���ο��� �� ���� ������� �ʽ��ϴ�.";
                break;
        }
    }

    public void OnConfirmPressed()
    {
        switch (curAction)
        {
            case PopupAction.Retry:
                SaveGame();
                GameManager.Instance().Scene.ChangeScene(MySceneManager.GetCurrentSceneName());
                break;
            case PopupAction.Escape:
                SaveGame();
                GameManager.Instance().Scene.ChangeScene(MySceneManager.SCENENAME.Exterior);
                break;
            case PopupAction.Reset:
                GameManager.Instance().Save.Reset();
                return;
            case PopupAction.Exit:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
#else
                Application.Quit();
#endif
                return;
        }
        GameManager.Instance().GameUnpause();

        static void SaveGame()
            => GameManager.Instance().Save.SaveToPrefs();
    }

    public void OnCancelPressed()
    {
        gameObject.SetActive(false);
    }

}
