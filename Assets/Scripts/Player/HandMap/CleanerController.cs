using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CleanerController : MonoBehaviour
{
    [SerializeField]
    private GameObject faucetPS = null;

    private Collider faucetZone = null;

    private PlayerManager player = null;

    private void Start()
    {
        faucetZone = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (!player) return;

        if (player.Map().isActiveAndEnabled)
            player.Map().WashUpdate(faucetZone);

        if (Vector3.Distance(player.xrOrigin.position, transform.position) > 2f) Deactivate();
    }

    private void Activate()
    {
        faucetPS.SetActive(true);
    }

    private void Deactivate()
    {
        faucetPS.SetActive(false);
        player = null;
    }

    public void OnHovered(XRBaseInteractable self)
    {
        var interactor = self.GetOldestInteractorHovering();
        if (interactor == null) return;
        var go = interactor.transform.root;
        player = go.GetComponent<PlayerManager>();
        if (player) Activate();
    }
}
