using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PlanTextureManager;

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

    [SerializeField]
    private Animator[] handleAnims = new Animator[2];

    [Header("Canvas")]
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private RectTransform photoOverlay;

    private MapPaperMeshHandler paperHandler;

    private float paperHeight = 0f;

    internal bool dissappearing = false;

    public PlanTextureManager PlanMgr { get; private set; } = null;

    internal Vector3 GetCentre()
        => Vector3.Lerp(handleLeft.position, handleRight.position, 0.5f);

    private void Awake()
    {
        PlanMgr = new PlanTextureManager(this);

        paperHandler = new MapPaperMeshHandler(this, handleLeft, handleRight, paperInitStats);
        paperHandler.SetMaterial(mapMaterial);
    }

    internal void UpdateTexture(Texture2D texture)
        => mapMaterial.SetTexture("_MapTexture", texture);

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
        float used = Vector3.Distance(handleLeft.position, handleRight.position) / (paperFullHeight * 1.5f);
        used = Mathf.Clamp01(used);
        foreach (var handleAnim in handleAnims)
            handleAnim.SetFloat("Used", used);
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
        effect.transform.SetPositionAndRotation(GetCentre(), Quaternion.identity);
        Destroy(effect, 1f);
    }

    #region LayDown

    internal bool layDown = false;

    private Transform XROrigin => player.xrOrigin;


    public void SetHeld(bool held)
    {
        layDown = !held;
        paperHandler.TogglePhysics(held);
        canvas.gameObject.SetActive(layDown);
        photoOverlay.gameObject.SetActive(false);
    }

    public void LaydownMap()
    {
        gameObject.SetActive(true);
        SetHeld(false);
        CancelInvoke(nameof(FoldLaydownMap));
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

    /// <summary>
    /// delay 초 뒤 지도를 접음
    /// </summary>
    public void RequestFoldLaydownMap(float delay = 0f)
    {
        if (delay == 0f) FoldLaydownMap();
        else Invoke(nameof(FoldLaydownMap), delay);
    }

    private void FoldLaydownMap()
    {
        layDown = false; // HandMapExpand의 접히는 애니메이션 허용
        canvas.gameObject.SetActive(false);
    }

    #region PhonePhoto

    public void UpdatePhotoProjection(Transform photo)
    {
        photoTransform = new PhotoTransform();

        #region Position
        Vector3 centerPos = Vector3.Lerp(handleLeft.position, handleRight.position, 0.5f);
        Vector3 localPos = photo.position - centerPos;

        photoTransform.offset = new Vector2(Vector3.Dot(localPos, handleLeft.right), Vector3.Dot(localPos, handleLeft.up)) / canvas.transform.localScale.x;
        photoOverlay.localPosition = photoTransform.offset;
        #endregion

        #region Rotation
        Vector3 planeNormal = handleLeft.rotation * Vector3.up;
        Quaternion projRot = Quaternion.FromToRotation(handleLeft.forward, planeNormal);
        Quaternion photoRot = Quaternion.LookRotation(photo.forward, planeNormal);
        Quaternion relativeRotation = projRot * photoRot;

        float projDegree = relativeRotation.eulerAngles.y;
        photoTransform.rotation = Mathf.Repeat(projDegree - 90f - handleLeft.rotation.eulerAngles.y, 360f) - 180f;

        photoOverlay.localRotation = Quaternion.Euler(0f, 0f, photoTransform.rotation);
        #endregion Rotation

        photoTransform.scale = 0.8f * photo.localScale.x;
        photoOverlay.localScale = photoTransform.scale * Vector3.one;

        photoOverlay.gameObject.SetActive(true);
    }

    private PhotoTransform photoTransform;

    public bool RequestPhotoAttach(Texture2D photo, Transform photoTF, float leniency)
    {
        UpdatePhotoProjection(photoTF);
        photoOverlay.gameObject.SetActive(false);

        if (GetDistanceFromMap(photoTF.position) > leniency) return false;
        PlanMgr.PastePhoto(photo, photoTransform);
        return true;
    }

    #endregion PhonePhoto

    private Vector2 lastPenOffset = Vector2.zero;
    private bool lastLine = false;

    public void PausePenDraw()
    {
        lastLine = false;
    }

    public void RequestPenDraw(Transform tip)
    {
        var dist = GetDistanceFromMap(tip.position);
        if (dist > 0.2f || dist < -0.1f) { PausePenDraw(); return; }

        Vector3 centerPos = Vector3.Lerp(handleLeft.position, handleRight.position, 0.5f);
        Vector3 localPos = tip.position - centerPos;

        Vector2 offset = new Vector2(Vector3.Dot(localPos, handleLeft.right), Vector3.Dot(localPos, handleLeft.up)) / canvas.transform.localScale.x;
        
        if (Vector2.Distance(lastPenOffset, offset) < 2f) return;
        if (!lastLine) lastPenOffset = offset; // new line
        PlanMgr.DrawPen(lastPenOffset, offset);
        lastPenOffset = offset;
        lastLine = true;
    }

    private float GetDistanceFromMap(Vector3 target)
    {
        Vector3 localPos = target - handleLeft.position;
        return Vector3.Dot(localPos, -handleLeft.forward);
    }

    #endregion LayDown

    /// <summary>
    /// 지도 리셋 기구가 부르는 업데이트
    /// </summary>
    /// <param name="washZone">지도가 리셋되기 위해 있어야하는 위치</param>
    public void WashUpdate(Collider washZone)
    {
        if (dissappearing) return;

        if (washZone.bounds.Contains(GetCentre()))
        {
            if (washTime < 0f) washTime = Time.time;
            if (washTime + 4f < Time.time)
            {
                PlanMgr.ResetPlan();
                CreateToggleEffect();
                washTime = float.MaxValue;
            }
        }
        else washTime = -1f;
    }

    private float washTime = -1f;

}
