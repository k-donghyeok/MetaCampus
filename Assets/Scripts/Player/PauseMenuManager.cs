using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
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
    }

    private bool lastMenuButton = false;

    private void Update()
    {
        if (Input.GetButton("XRI_Left_MenuButton"))
        {
            if (!lastMenuButton)
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
            lastMenuButton = true;
        }
        else lastMenuButton = false;
    }

    private void ActivateMenu()
    {
        popup.gameObject.SetActive(false);
        menuObject.SetActive(true);
        menuObject.transform.SetParent(PlayerManager.InstanceOrigin());
        menuObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public void OnButtonResumePressed()
    {
        if (popup.gameObject.activeSelf) return; // 무시
        popup.gameObject.SetActive(false);
        menuObject.SetActive(false);
        menuObject.transform.SetParent(transform);
        menuObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        GameManager.Instance().GameUnpause();
    }

    public void OnRetryButtonPressed() => OnButtonPopupRequested(PopupAction.Retry);
    public void OnEscapeButtonPressed() => OnButtonPopupRequested(PopupAction.Escape);
    public void OnResetButtonPressed() => OnButtonPopupRequested(PopupAction.Reset);


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
        Reset
    }
}
