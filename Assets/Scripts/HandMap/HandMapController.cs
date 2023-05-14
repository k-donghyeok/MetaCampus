using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMapController : MonoBehaviour
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

    private void Start()
    {
        paperHandler = new MapPaperMeshHandler(this, handleLeft, handleRight, paperInitStats);
        paperHandler.SetMaterial(mapMaterial);
    }

    private void Update()
    {
        paperHeight = Mathf.Lerp(paperHeight, paperFullHeight, 0.1f);
        paperHandler.Update(paperHeight);
    }

    private void FixedUpdate()
    {
        paperHandler.FixedUpdate(paperHeight);
    }
}
