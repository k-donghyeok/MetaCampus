using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMapManager : MonoBehaviour
{
    [SerializeField]
    private Transform handleLeft = null;
    [SerializeField]
    private Transform handleRight = null;

    [Header("Paper")]
    [SerializeField]
    private MapPaperMeshHandler.PaperInfo paperInitStats;

    [SerializeField, Range(0f, 2f)]
    private float paperFullHeight = 0.5f;

    [SerializeField]
    private Material mapMaterial;

    private MapPaperMeshHandler paperHandler;

    private float paperHeight = 0f;

    private bool dissappearing = false;

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

    private void Start()
    {
        paperHandler = new MapPaperMeshHandler(this, handleLeft, handleRight, paperInitStats);
        paperHandler.SetMaterial(mapMaterial);
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
            if (paperHeight < 0.001f) gameObject.SetActive(false);
        }
        handleLeft.localScale = new(1f, paperHeight, 1f);
        handleRight.localScale = new(1f, paperHeight, 1f);
        paperHandler.Update(paperHeight);
    }

    private void FixedUpdate()
    {
        paperHandler.FixedUpdate(paperHeight);
    }
}
