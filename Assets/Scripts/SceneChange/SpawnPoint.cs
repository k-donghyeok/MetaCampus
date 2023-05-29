using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private int myID = 0;

    [SerializeField] private Transform spawnPoint = null;

    public Vector3 GetSpawnPos() => spawnPoint.position;

    public int GetID() => myID;
    
    protected virtual void PassThrough()
    {
        GameManager.Instance().Spawn.SaveSpawnPoint(GetID());
    }

    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("충돌");
        if (!_other.transform.parent.CompareTag("Player")) return;
        Debug.Log("플레이어 맞음");
        PassThrough();
    }
}
