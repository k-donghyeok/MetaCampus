using System.Collections;
using UnityEngine;

public class MapPaperMeshHandler
{
    public MapPaperMeshHandler(HandMapController controller, Transform handleLeft, Transform handleRight)
    {
        this.controller = controller;
        this.handleLeft = handleLeft;
        this.handleRight = handleRight;
    }

    private readonly HandMapController controller;

    private readonly Transform handleLeft;
    private readonly Transform handleRight;

    public void Update()
    {
        Debug.DrawLine(handleLeft.position, handleRight.position);
    }
}