using UnityEngine;

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

    public void OnHovered()
    {
        player = PlayerManager.Instance();
        if (player) Activate();
    }
}
