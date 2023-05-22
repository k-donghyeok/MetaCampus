using System.Linq;
using UnityEngine;

public class HandMapManager : MonoBehaviour
{
    [SerializeField]
    internal Transform xrOrigin = null;

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

    private MapPaperMeshHandler paperHandler;

    private float paperHeight = 0f;

    internal bool dissappearing = false;

    internal bool layDown = false;

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
    }

    private void OnDisable()
    {
        paperHandler.SetActive(false);
    }

    public void SetLaydown(bool held)
    {
        layDown = !held;
        paperHandler.TogglePhysics(held);
    }

    public void LaydownMap()
    {
        gameObject.SetActive(true);
        SetLaydown(false);
        CreateToggleEffect();
        handleLeft.transform.SetParent(xrOrigin);
        handleLeft.transform.SetLocalPositionAndRotation(
            new Vector3(-0.6f, 1.2f, 0.5f),
            Quaternion.Euler(40f, 0f, 0f));
        handleRight.transform.SetParent(xrOrigin);
        handleRight.transform.SetLocalPositionAndRotation(
            new Vector3(0.6f, 1.2f, 0.5f),
            Quaternion.Euler(40f, 0f, 0f));
    }

    public void FoldLaydownMap()
    {
        layDown = false; // HandMapExpand의 접히는 애니메이션 허용
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
        handleLeft.localScale = new(1f, paperHeight, 1f);
        handleRight.localScale = new(1f, paperHeight, 1f);
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

}
