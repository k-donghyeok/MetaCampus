using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private int myID = -1;

    [SerializeField]
    private Transform spawnPoint = null;

    public Vector3 GetSpawnPos() => spawnPoint.position;

    public int GetID() => myID;
    
    private void PassThrough()
    {
        GameManager.Instance().Spawn.SaveSpawnPoint(GetID());
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (!_other.transform.root.CompareTag("Player")) return;
        PassThrough();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(spawnPoint.position, 1f);
    }
}
