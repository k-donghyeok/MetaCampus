using UnityEngine;

public class RespawnPointTrigger : MonoBehaviour
{
    public int respawnPointID; // ID of the respawn point
    public int sceneID; // ID of the connected scene
    private bool canSave = true; // Flag to control save frequency

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && canSave)
        {
            GameManager.Instance.UpdateRespawnPoint(respawnPointID, sceneID);
            canSave = false; // Disable saving temporarily
            Invoke(nameof(EnableSave), 1f); // Enable saving after 1 second
        }
    }

    private void EnableSave()
    {
        canSave = true;
    }
}
