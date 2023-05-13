using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMapController : MonoBehaviour
{
    [SerializeField]
    private Transform handleLeft = null;
    [SerializeField]
    private Transform handleRight = null;

    [SerializeField]
    private MapPaperMeshHandler.PaperInfo paperInitStats;

    [SerializeField]
    private Material mapMaterial;

    private MapPaperMeshHandler paperHandler;

    private void Start()
    {
        paperHandler = new MapPaperMeshHandler(this, handleLeft, handleRight, paperInitStats);
        paperHandler.SetMaterial(mapMaterial);
    }

    private void Update()
    {
        paperHandler.Update();
    }

    private void FixedUpdate()
    {
        paperHandler.FixedUpdate();
    }
}
