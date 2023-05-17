using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTree : MonoBehaviour
{
    Collider myCollider;

    private void Start()
    {
        myCollider = GetComponent<Collider>();
        Debug.Log($"Test: {myCollider}");
    }

    void Update()
    {
        if (!Input.GetMouseButton(0)) return;
        if(! Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var info)) return;
        if (info.collider != myCollider) return;
        gameObject.SetActive(false);

    }
}
