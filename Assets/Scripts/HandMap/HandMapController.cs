using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMapController : MonoBehaviour
{
    [SerializeField]
    private Transform handleLeft = null;
    [SerializeField]
    private Transform handleRight = null;

    [Header("Paper - InitValue")]
    [SerializeField, Range(0.01f, 1f)]
    private float paperHeight = 0.3f;

    [SerializeField, Range(2, 50)]
    private int paperSegsHorz = 5;
    [SerializeField, Range(2, 50)]
    private int paperSegsVert = 5;

    [SerializeField]
    private Material paperMaterial = null;

    private MapPaperMeshHandler paperHandler;

    private void Start()
    {
        paperHandler = new MapPaperMeshHandler(this, handleLeft, handleRight, paperHeight, paperSegsHorz, paperSegsVert);
        paperHandler.Renderer().material = paperMaterial;
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
