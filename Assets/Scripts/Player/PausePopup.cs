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
                txtAction.text = "재시도하시겠습니까?";
                txtMessage.text = "이 건물 내에서의\n진행사항이 저장되지 않습니다.";
                break;
            case PopupAction.Escape:
                txtAction.text = "나가시겠습니까?";
                txtMessage.text = "이 건물 내에서의\n진행사항이 저장되지 않습니다.";
                break;
            case PopupAction.Reset:
                txtAction.text = "세이브 초기화하겠습니까?";
                txtMessage.text = "모든 진행사항이 삭제됩니다.";
                break;
            case PopupAction.Exit:
                txtAction.text = "게임을 종료하시겠습니까?";
                txtMessage.text = StageManager.Instance().IsExterior() ? "" : "내부에서 한 것은 저장되지 않습니다.";
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
