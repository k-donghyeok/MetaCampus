using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMapManager : MonoBehaviour
{
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

    private void Awake()
    {
        paperHandler = new MapPaperMeshHandler(this, handleLeft, handleRight, paperInitStats);
        paperHandler.SetMaterial(mapMaterial);
    }

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


    private void Update()
    {
        if (!dissappearing)
        {
            paperHeight = Mathf.Lerp(paperHeight, paperFullHeight, 0.1f);
            if (paperHeight > paperFullHeight * 0.999f) paperHeight = paperFullHeight;
        }
        else
        {
            paperHeight = Mathf.Lerp(paperHeight, 0f, 0.1f);
            if (paperHeight < 0.01f) DisableMap();
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
