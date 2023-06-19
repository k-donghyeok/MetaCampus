using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField]
    private InputActionReference input;

    [SerializeField]
    private GameObject menuObject;

    [SerializeField]
    private TMP_Text txtStage;

    [SerializeField]
    private Button[] interiorButtons;

    [SerializeField]
    private PausePopup popup;

    private void Start()
    {
        var scene = MySceneManager.GetCurrentSceneName();
        txtStage.text = $"~ {MySceneManager.GetDisplaySceneName(scene)} ~";
        if (scene == MySceneManager.SCENENAME.Exterior || scene == MySceneManager.SCENENAME.Tutorial)
            foreach (var btn in interiorButtons) btn.interactable = false;

        popup.gameObject.SetActive(false);
        menuObject.SetActive(false);
        input.action.started += ToggleMenu;
    }

    private void OnDestroy()
    {
        input.action.started -= ToggleMenu;
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance().Paused)
        {
            ActivateMenu();
            GameManager.Instance().GamePause();
        }
        else
        {
            OnButtonResumePressed();
        }
    }

    private void ActivateMenu()
    {
        popup.gameObject.SetActive(false);
        menuObject.SetActive(true);
        transform.SetParent(PlayerManager.InstanceOrigin());
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public void OnButtonResumePressed()
    {
        if (popup.gameObject.activeSelf) return; // 무시
        popup.gameObject.SetActive(false);
        menuObject.SetActive(false);
        transform.SetParent(PlayerManager.Instance().transform);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        GameManager.Instance().GameUnpause();
    }

    public void OnRetryButtonPressed() => OnButtonPopupRequested(PopupAction.Retry);
    public void OnEscapeButtonPressed() => OnButtonPopupRequested(PopupAction.Escape);
    public void OnResetButtonPressed() => OnButtonPopupRequested(PopupAction.Reset);
    public void OnExitButtonPressed() => OnButtonPopupRequested(PopupAction.Exit);


    private void OnButtonPopupRequested(PopupAction action)
    {
        if (popup.gameObject.activeSelf) return; // 무시
        popup.gameObject.SetActive(true);
        popup.SetAction(action);
    }

    public enum PopupAction
    {
        Retry,
        Escape,
        Reset,
        Exit
    }
}
