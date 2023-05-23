using System.Linq;
using UnityEngine;

public class HandMapManager : MonoBehaviour
{
    internal PlayerManager player = null;

    internal PhoneManager Phone => player.Phone();

    [SerializeField]
    internal Transform handleLeft = null;
    [SerializeField]
    internal Transform handleRight = null;

    [Header("Paper")]
    [SerializeField]
    private MapPaperMeshHandler.PaperInfo paperInitStats;

    [SerializeField, Range(0f, 2f)]
    private float paperFullHeight = 0.5f;

    [SerializeField]
    private Material mapMaterial;

    [SerializeField]
    private GameObject prefabParticleEffect = null;

    [Header("Canvas")]
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private RectTransform photoOverlay;

    private MapPaperMeshHandler paperHandler;

    private float paperHeight = 0f;

    internal bool dissappearing = false;

    public PlanTextureManager PlanMgr { get; private set; } = null;

    private void Awake()
    {
        PlanMgr = new PlanTextureManager(this);

        paperHandler = new MapPaperMeshHandler(this, handleLeft, handleRight, paperInitStats);
        paperHandler.SetMaterial(mapMaterial);
    }

    internal void UpdateTexture(Texture2D texture)
        => mapMaterial.SetTexture("MapTexture", texture);

    private void OnEnable()
    {
        paperHeight = 0f;
        dissappearing = false;
        paperHandler.SetActive(true);
        canvas.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        paperHandler.SetActive(false);
    }


    private void Update()
    {
        if (!dissappearing)
        {
            paperHeight = Mathf.Lerp(paperHeight, paperFullHeight, 5f * Time.deltaTime);
            if (paperHeight > paperFullHeight * 0.999f) paperHeight = paperFullHeight;
        }
        else
        {
            paperHeight -= (1.1f - paperHeight) * 3f * Time.deltaTime;
            if (paperHeight < 0.01f) { paperHeight = 0f; DisableMap(); }
        }
        handleLeft.localScale = new(1f, paperHeight / paperFullHeight, 1f);
        handleRight.localScale = new(1f, paperHeight / paperFullHeight, 1f);
        paperHandler.Update(paperHeight);
    }

    private void FixedUpdate()
    {
        paperHandler.FixedUpdate(paperHeight);
    }

    private void DisableMap()
    {
        handleLeft.transform.SetParent(transform);
        handleRight.transform.SetParent(transform);
        gameObject.SetActive(false);
        CreateToggleEffect();
    }

    public void CreateToggleEffect()
    {
        var effect = Instantiate(prefabParticleEffect);
        effect.transform.SetPositionAndRotation(Vector3.Lerp(handleLeft.transform.position, handleRight.transform.position, 0.5f), Quaternion.identity);
        Destroy(effect, 1f);
    }

    #region LayDown

    internal bool layDown = false;

    private Transform XROrigin => player.xrOrigin;


    public void SetLaydown(bool held)
    {
        layDown = !held;
        paperHandler.TogglePhysics(held);
        canvas.gameObject.SetActive(layDown);
        photoOverlay.gameObject.SetActive(false);
    }

    public void LaydownMap()
    {
        gameObject.SetActive(true);
        SetLaydown(false);
        CreateToggleEffect();
        handleLeft.transform.SetParent(XROrigin);
        handleLeft.transform.SetLocalPositionAndRotation(
            new Vector3(-0.35f, 0.8f, 0.5f),
            Quaternion.Euler(50f, 0f, 0f));
        handleRight.transform.SetParent(XROrigin);
        handleRight.transform.SetLocalPositionAndRotation(
            new Vector3(0.35f, 0.8f, 0.5f),
            Quaternion.Euler(50f, 0f, 0f));
    }

    public void FoldLaydownMap()
    {
        layDown = false; // HandMapExpand�� ������ �ִϸ��̼� ���
        canvas.gameObject.SetActive(false);
    }

    public void UpdatePhotoProjection(Transform photo)
    {
        #region Position
        Vector3 centerPos = Vector3.Lerp(handleLeft.position, handleRight.position, 0.5f);
        Vector3 localPos = photo.position - centerPos;

        Vector2 offset2D = new(Vector3.Dot(localPos, handleLeft.right), Vector3.Dot(localPos, handleLeft.up));

        photoOverlay.localPosition = offset2D / canvas.transform.localScale.x;
        #endregion

        #region Rotation
        Vector3 planeNormal = handleLeft.rotation * Vector3.up;
        Quaternion projRot = Quaternion.FromToRotation(handleLeft.forward, planeNormal);
        Quaternion photoRot = Quaternion.LookRotation(photo.forward, planeNormal);
        Quaternion relativeRotation = projRot * photoRot;

        float projDegree = relativeRotation.eulerAngles.y;
        projDegree = Mathf.Repeat(projDegree + 90f, 360f) - 180f;

        photoOverlay.localRotation = Quaternion.Euler(0f, 0f, projDegree);
        #endregion Rotation

        photoOverlay.localScale = 0.8f * photo.localScale.x * Vector3.one;

        photoOverlay.gameObject.SetActive(true);
    }

    #endregion LayDown

}
