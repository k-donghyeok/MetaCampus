using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMapController : MonoBehaviour
{
    [SerializeField]
    private Transform handleLeft = null;
    [SerializeField]
    private Transform handleRight = null;

    private MapPaperMeshHandler meshHandler;

    private void Start()
    {
        meshHandler = new MapPaperMeshHandler(this, handleLeft, handleRight);
    }

    private void Update()
    {
        meshHandler.Update();
    }
}
