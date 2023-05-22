using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerActivator : MonoBehaviour
{
    private Spawner owner;

    private void Start()
    {
        owner = GetComponentInParent<Spawner>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        owner.Activate();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        owner.Deactivate();
    }
}
