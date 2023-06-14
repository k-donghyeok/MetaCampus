using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// �������� �������� �ൿ ����
/// </summary>
public class PhoneManager : MonoBehaviour
{
    internal PlayerManager player = null;

    internal HandMapManager Map => player.Map();

    [Header("Capture")]
    [SerializeField]
    private GameObject captureGO = null;
    [SerializeField]
    private Camera cam = null;
    [SerializeField]
    private MeshRenderer screen = null;

    [Header("Attach")]
    [SerializeField]
    internal GameObject attachGO = null;
    [SerializeField]
    internal Transform[] stretchers = new Transform[2];
    [SerializeField]
    internal MeshRenderer photoScreen = null;

    public CaptureBehaviour CaptureBehav { get; private set; } = null;
    public AttachBehaviour AttachBehav { get; private set; } = null;

    public Texture2D Photo { get; private set; } = null;

    public void UpdatePhoto(Texture2D newPhoto)
    {
        Photo = newPhoto;
        AttachBehav.UpdatePhoto(Photo);
    }

    private void Awake()
    {
        CaptureBehav = new CaptureBehaviour(this, cam);
        AttachBehav = new AttachBehaviour(this);
    }

    /// <summary>
    /// �̰��� �÷��̾� �տ� ����ְ�, ��ȣ�ۿ��� �������� ����
    /// </summary>
    public bool Held { get; private set; }

    private float hideTimer = 1f;

    private InputDevice heldDevice;

    /// <summary>
    /// �����⸦ ���� ��Ʈ�ѷ� ����
    /// </summary>
    /// <param name="device"></param>
    public void SetHeld(InputDevice device)
    {
        heldDevice = device;
        SetHeld(true);
    }

    /// <summary>
    /// �����⸦ ����
    /// </summary>
    /// <param name="held">�����⸦ ���� ����</param>
    public void SetHeld(bool held = false)
    {
        if (Held == held) return;
        Held = held;
        hideTimer = 1f;
        if (held) ChangeMode(Mode.Capture);
        else if (CurMode == Mode.Attach)
        {
            foreach (var s in stretchers) s.gameObject.SetActive(false);
            Map.RequestFoldLaydownMap(0f);
        }
    }

    public void AttachAction()
    {
        SetHeld(false);
        Map.RequestFoldLaydownMap(2f);
        hideTimer = 0f;
    }

    private void Update()
    {
        if (!Held)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0f) gameObject.SetActive(false);
            return;
        }
        if (!heldDevice.isValid) return;

        switch (CurMode)
        {
            case Mode.Capture:
                CaptureBehav.Update(heldDevice);
                break;
            case Mode.Attach:
                AttachBehav.Update(heldDevice);
                if (stretched)
                {
                    attachGO.transform.localScale = Vector3.one * GetStretch();
                    attachGO.transform.position = Vector3.Lerp(stretchers[0].position, stretchers[1].position, 0.53846f);
                    //attachGO.transform.localRotation = Vector3.MoveTowards(stretchers[0].position, stretchers[1].position, 1f);
                }
                break;
        }

    }

    public Mode CurMode { get; private set; } = Mode.Capture;

    public void ChangeMode(Mode newMode)
    {
        if (CurMode == newMode) return;

        switch (CurMode)
        {
            case Mode.Capture:
                cam.gameObject.SetActive(false);
                captureGO.SetActive(false);
                break;
            case Mode.Attach:
                attachGO.SetActive(false);
                break;
        }
        foreach (var s in stretchers) s.gameObject.SetActive(false);
        switch (newMode)
        {
            case Mode.Capture:
                cam.gameObject.SetActive(true);
                captureGO.SetActive(true);
                break;
            case Mode.Attach:
                Map.LaydownMap();
                attachGO.SetActive(true);
                attachGO.transform.localScale = Vector3.one;
                attachGO.transform.localPosition = Vector3.zero;
                stretchers[0].transform.localPosition = new Vector3(0f, 0f, -0.14f);
                stretchers[1].transform.localPosition = new Vector3(0f, 0f, 0.12f);
                //Debug.Log($"heldDevice {heldDevice.isValid}: characteristics {heldDevice.characteristics}");
                if (heldDevice.isValid)
                    switch (GrabActionHandler.GetHand(heldDevice))
                    {
                        case 0: stretchers[1].gameObject.SetActive(true); break;
                        case 1: stretchers[0].gameObject.SetActive(true); break;
                    }
                break;
        }

        CurMode = newMode;
    }

    public enum Mode
    {
        Capture,
        Attach
    }

    #region AttachStretch

    public void OnStretcherHeld(XRBaseInteractable stretcher)
    {
        stretched = true;
        //this.stretcher = stretcher.transform;
    }

    public void OnStretcherUnheld()
    {
        stretched = false;
        //stretcher = null;
    }

    private float GetStretch()
    {
        float dist = Vector3.Distance(stretchers[0].position, stretchers[1].position);
        return dist / 0.26f;
    }

    private bool stretched = false;

    //private Transform stretcher = null;

    #endregion AttachStretch
}
